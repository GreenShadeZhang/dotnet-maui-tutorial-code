using Mp4EmojisPlayer.Models;
using Mp4EmojisPlayer.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core.Primitives;

namespace Mp4EmojisPlayer
{
    public partial class MainPage : ContentPage
    {
        private readonly EmotionService _emotionService;
        private readonly IVideoService _videoService;
        private List<EmotionModel> _allEmotions = new();
        private bool _isPlaying = false;
        private bool _isLoadingVideo = false;
        private CancellationTokenSource? _playbackCancellationTokenSource;

        public MainPage(EmotionService emotionService, IVideoService videoService)
        {
            InitializeComponent();
            _emotionService = emotionService;
            _videoService = videoService;
            LoadEmotions();
        }

        private async void LoadEmotions()
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                
                _allEmotions = await _emotionService.LoadEmotionsAsync();
                EmotionsCollectionView.ItemsSource = _allEmotions;
                
                LoadingIndicator.IsVisible = false;
                
                // 减少调试输出，只在Debug模式下执行
#if DEBUG
                // 异步执行调试信息，不阻塞UI
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var availableVideos = await DebugHelper.ListAvailableVideosAsync();
                        System.Diagnostics.Debug.WriteLine($"Found {availableVideos.Count} available video files:");
                        foreach (var video in availableVideos.Take(3)) // 只显示前3个
                        {
                            System.Diagnostics.Debug.WriteLine($"  - {video}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Debug info error: {ex.Message}");
                    }
                });
#endif
                
                // 暂时移除默认播放，避免启动时的问题
                System.Diagnostics.Debug.WriteLine($"Loaded {_allEmotions.Count} emotions successfully");
            }
            catch (Exception ex)
            {
                LoadingIndicator.IsVisible = false;
                await DisplayAlert("错误", $"加载表情失败: {ex.Message}", "确定");
            }
        }

        private async void OnQuickEmotionClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string categoryName)
            {
                var emotion = _emotionService.GetRandomEmotionByCategory(categoryName);
                if (emotion != null)
                {
                    PlayEmotion(emotion);
                }
                else
                {
                    await DisplayAlert("提示", $"该分类 '{categoryName}' 暂无可用表情", "确定");
                }
            }
        }

        private void OnRandomEmotionClicked(object sender, EventArgs e)
        {
            if (_allEmotions.Any())
            {
                var random = new Random();
                var randomEmotion = _allEmotions[random.Next(_allEmotions.Count)];
                PlayEmotion(randomEmotion);
            }
        }

        private void OnEmotionSelected(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is EmotionModel emotion)
            {
                PlayEmotion(emotion);
            }
        }

        private async void PlayEmotion(EmotionModel emotion)
        {
            // 防止重复点击和并发播放
            if (_isLoadingVideo)
            {
                System.Diagnostics.Debug.WriteLine("Already loading a video, ignoring request");
                return;
            }

            try
            {
                _isLoadingVideo = true;
                
                // 显示加载指示器
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PlaybackIndicator.IsVisible = true;
                    PlaybackIndicator.IsRunning = true;
                    CurrentEmotionLabel.Text = emotion.DisplayName;
                });
                
                // 取消当前播放任务（如果有）
                _playbackCancellationTokenSource?.Cancel();
                _playbackCancellationTokenSource = new CancellationTokenSource();
                
                // 停止当前播放
                await StopCurrentPlaybackAsync();
                
                var videoFileName = $"{emotion.CmdTag}.mp4";
                System.Diagnostics.Debug.WriteLine($"Attempting to play: {videoFileName}");
                
                // 检查视频文件是否存在
                bool videoExists = false;
                try
                {
                    videoExists = await _videoService.VideoExistsAsync(videoFileName);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error checking video existence: {ex.Message}");
                }
                
                if (videoExists)
                {
                    // 使用安全的视频源设置方法
                    var success = await SetVideoSourceSafelyAsync(videoFileName, _playbackCancellationTokenSource.Token);
                    
                    if (success && !_playbackCancellationTokenSource.Token.IsCancellationRequested)
                    {
                        _isPlaying = true;
                        System.Diagnostics.Debug.WriteLine($"Successfully started playing: {emotion.DisplayName} ({emotion.CmdTag})");
                    }
                    else if (_playbackCancellationTokenSource.Token.IsCancellationRequested)
                    {
                        System.Diagnostics.Debug.WriteLine("Playback was cancelled");
                    }
                    else
                    {
                        var errorMsg = $"无法播放视频文件 {videoFileName}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("播放错误", errorMsg, "确定");
                        });
                    }
                }
                else
                {
                    var errorMsg = $"视频文件 {videoFileName} 不存在";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    
#if DEBUG
                    // 只在Debug模式下输出详细诊断信息
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var diagnosis = await DebugHelper.DiagnoseVideoPathsAsync(videoFileName);
                            System.Diagnostics.Debug.WriteLine(diagnosis);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Debug diagnosis error: {ex.Message}");
                        }
                    });
#endif
                    
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("文件不存在", errorMsg, "确定");
                    });
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error playing emotion: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMsg);
                
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("播放错误", $"无法播放表情 {emotion.DisplayName}: {ex.Message}", "确定");
                });
            }
            finally
            {
                _isLoadingVideo = false;
                // 隐藏加载指示器
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PlaybackIndicator.IsVisible = false;
                    PlaybackIndicator.IsRunning = false;
                });
            }
        }

        /// <summary>
        /// 停止当前播放的视频
        /// </summary>
        private async Task StopCurrentPlaybackAsync()
        {
            if (_isPlaying)
            {
                try
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        VideoPlayer.Stop();
                    });
                    
                    _isPlaying = false;
                    System.Diagnostics.Debug.WriteLine("Stopped current playback");
                    
                    // 给一点时间让MediaElement完全停止
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error stopping playback: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 安全地设置MediaElement的视频源，包含多种方式的回退机制
        /// </summary>
        private async Task<bool> SetVideoSourceSafelyAsync(string videoFileName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return false;

                System.Diagnostics.Debug.WriteLine($"Setting video source for: {videoFileName}");
                
                // 确保当前播放已完全停止
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    VideoPlayer.Stop();
                });
                
                // 短暂延迟确保停止操作完成
                await Task.Delay(50, cancellationToken);
                
                // 方法1: 使用MediaSource.FromResource() - 这是官方推荐的方式
                try
                {
                    var resourcePath = $"videos/{videoFileName}";
                    
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        VideoPlayer.Source = MediaSource.FromResource(resourcePath);
                    });
                    
                    System.Diagnostics.Debug.WriteLine($"Using MediaSource.FromResource with path: {resourcePath}");
                    
                    if (cancellationToken.IsCancellationRequested)
                        return false;
                    
                    // 等待一小段时间让MediaElement准备
                    await Task.Delay(100, cancellationToken);
                    
                    if (cancellationToken.IsCancellationRequested)
                        return false;
                    
                    // 开始播放
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        VideoPlayer.Play();
                    });
                    
                    return true;
                }
                catch (OperationCanceledException)
                {
                    return false;
                }
                catch (Exception resourceEx)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return false;
                        
                    System.Diagnostics.Debug.WriteLine($"MediaSource.FromResource failed: {resourceEx.Message}");
                    
                    // 方法2: 使用平台特定的URI
                    try
                    {
                        var videoUri = await _videoService.GetVideoUriAsync(videoFileName);
                        
                        if (!string.IsNullOrEmpty(videoUri) && !cancellationToken.IsCancellationRequested)
                        {
                            System.Diagnostics.Debug.WriteLine($"Trying platform-specific URI: {videoUri}");
                            
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                VideoPlayer.Source = MediaSource.FromUri(videoUri);
                            });
                            
                            await Task.Delay(100, cancellationToken);
                            
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                MainThread.BeginInvokeOnMainThread(() =>
                                {
                                    VideoPlayer.Play();
                                });
                                return true;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to get platform-specific URI or cancelled");
                        }
                    }
                    catch (Exception uriEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Platform URI method failed: {uriEx.Message}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Video loading was cancelled");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting video source: {ex.Message}");
            }
            
            return false;
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Media playback ended");
            // 视频播放结束后停止，确保不循环播放
            if (sender is MediaElement mediaElement)
            {
                mediaElement.Stop();
                _isPlaying = false;
                System.Diagnostics.Debug.WriteLine("Video playback stopped after completion");
                
                // 隐藏播放指示器
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PlaybackIndicator.IsVisible = false;
                    PlaybackIndicator.IsRunning = false;
                });
            }
        }

        private async void OnMediaFailed(object sender, EventArgs e)
        {
            _isPlaying = false;
            var errorMsg = "Media failed to load";
            if (sender is MediaElement mediaElement)
            {
                errorMsg += $" - Source: {mediaElement.Source}";
            }
            
            System.Diagnostics.Debug.WriteLine(errorMsg);
            
            // 隐藏播放指示器
            PlaybackIndicator.IsVisible = false;
            PlaybackIndicator.IsRunning = false;
            
            await DisplayAlert("播放失败", "视频加载失败，请检查文件路径和格式", "确定");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            try
            {
                // 页面消失时取消所有播放任务
                _playbackCancellationTokenSource?.Cancel();
                
                // 停止视频播放
                VideoPlayer.Stop();
                _isPlaying = false;
                PlaybackIndicator.IsVisible = false;
                PlaybackIndicator.IsRunning = false;
                
                System.Diagnostics.Debug.WriteLine("Page disappearing - cleaned up resources");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping video on page disappearing: {ex.Message}");
            }
        }
    }
}
