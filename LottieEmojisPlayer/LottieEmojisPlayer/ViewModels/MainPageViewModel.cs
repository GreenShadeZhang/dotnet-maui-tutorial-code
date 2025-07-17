using LottieEmojisPlayer.Controls;
using LottieEmojisPlayer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LottieEmojisPlayer.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private LottieFileItem? _selectedLottieFile;
        private bool _isPlaying;
        private string _animationInfo = string.Empty;
        private LottieAnimationView? _lottieView;
        #endregion

        #region Constructor
        public MainPageViewModel()
        {
            LottieFiles = new ObservableCollection<LottieFileItem>();
            PlayCommand = new Command(OnPlay, () => SelectedLottieFile != null);
            StopCommand = new Command(OnStop, () => SelectedLottieFile != null);
            LoadLottieFilesAsync();
        }
        #endregion

        #region Properties
        public ObservableCollection<LottieFileItem> LottieFiles { get; }

        public LottieFileItem? SelectedLottieFile
        {
            get => _selectedLottieFile;
            set
            {
                if (_selectedLottieFile != value)
                {
                    // 取消上一个选中项
                    if (_selectedLottieFile != null)
                        _selectedLottieFile.IsSelected = false;

                    _selectedLottieFile = value;
                    
                    // 设置新的选中项
                    if (_selectedLottieFile != null)
                        _selectedLottieFile.IsSelected = true;

                    OnPropertyChanged();
                    ((Command)PlayCommand).ChangeCanExecute();
                    ((Command)StopCommand).ChangeCanExecute();
                }
            }
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        public string AnimationInfo
        {
            get => _animationInfo;
            set
            {
                _animationInfo = value;
                OnPropertyChanged();
            }
        }

        public LottieAnimationView? LottieView
        {
            get => _lottieView;
            set
            {
                if (_lottieView != value)
                {
                    if (_lottieView != null)
                    {
                        _lottieView.OnStop -= OnAnimationStopped;
                        _lottieView.PropertyChanged -= OnLottieViewPropertyChanged;
                    }

                    _lottieView = value;

                    if (_lottieView != null)
                    {
                        _lottieView.OnStop += OnAnimationStopped;
                        _lottieView.PropertyChanged += OnLottieViewPropertyChanged;
                    }

                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }
        #endregion

        #region Private Methods
        private async void LoadLottieFilesAsync()
        {
            try
            {
                var lottieFilesPath = "lottiefiles";
                
                // 获取所有 JSON 文件
                var files = await GetLottieFilesFromRawAsync(lottieFilesPath);
                
                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    LottieFiles.Add(new LottieFileItem(fileName, file));
                }

                // 默认选择第一个文件
                if (LottieFiles.Count > 0)
                {
                    SelectedLottieFile = LottieFiles[0];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading lottie files: {ex.Message}");
            }
        }

        private async Task<List<string>> GetLottieFilesFromRawAsync(string folderPath)
        {
            var files = new List<string>();
            
            try
            {
                // 在 MAUI 中，Raw 资源文件会被嵌入到应用包中
                // 我们需要通过文件系统 API 来获取文件列表
                
                // 由于 MAUI 的限制，我们直接列出已知的文件
                var knownFiles = new[]
                {
                    "ask.json",
                    "h0001.mp4.lottie.json", 
                    "h0065.mp4.lottie.json",
                    "look.json",
                    "sad.json",
                    "speak.json",
                    "think.json"
                };

                foreach (var file in knownFiles)
                {
                    var fullPath = Path.Combine(folderPath, file);
                    try
                    {
                        // 验证文件是否存在
                        using var stream = await FileSystem.OpenAppPackageFileAsync(fullPath);
                        if (stream != null)
                        {
                            files.Add(fullPath);
                        }
                    }
                    catch
                    {
                        // 文件不存在，跳过
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting lottie files: {ex.Message}");
            }

            return files;
        }

        private void OnPlay()
        {
            LottieView?.PlayAnimation();
        }

        private void OnStop()
        {
            LottieView?.StopAnimation();
        }

        private void OnAnimationStopped(object? sender, EventArgs e)
        {
            IsPlaying = false;
        }

        private void OnLottieViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LottieAnimationView.IsPlaying))
            {
                IsPlaying = LottieView?.IsPlaying ?? false;
            }
            else if (e.PropertyName == nameof(LottieAnimationView.Info) && LottieView?.Info != null)
            {
                var info = LottieView.Info;
                AnimationInfo = $"版本: {info.Version}\n" +
                              $"时长: {info.Duration.TotalSeconds:F2}s\n" +
                              $"帧率: {info.Fps:F1} FPS\n" +
                              $"起始帧: {info.InPoint}\n" +
                              $"结束帧: {info.OutPoint}";
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
