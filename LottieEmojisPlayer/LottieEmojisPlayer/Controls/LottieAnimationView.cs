using LottieEmojisPlayer.Models;
using SkiaSharp;
using SkiaSharp.Resources;
using SkiaSharp.Skottie;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace LottieEmojisPlayer.Controls
{
    /// <summary>
    /// A MAUI control for displaying and controlling Lottie (JSON-based) animations using SkiaSharp.
    /// </summary>
    public class LottieAnimationView : SKCanvasView, IDisposable
    {
        #region Private Fields
        private readonly Stopwatch _watch = new();
        private SkiaSharp.Skottie.Animation? _animation;
        private IDispatcherTimer? _timer;
        private int _loopCount;
        private bool _disposedValue;
        #endregion

        #region Bindable Properties

        public static readonly BindableProperty FilePathProperty =
            BindableProperty.Create(nameof(FilePath), typeof(string), typeof(LottieAnimationView), null, propertyChanged: OnFilePathChanged);

        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(LottieAnimationView), false);

        public static readonly BindableProperty RepeatCountProperty =
            BindableProperty.Create(nameof(RepeatCount), typeof(int), typeof(LottieAnimationView), 0, propertyChanged: OnRepeatCountChanged);

        public static readonly BindableProperty IsPlayingProperty =
            BindableProperty.Create(nameof(IsPlaying), typeof(bool), typeof(LottieAnimationView), false, BindingMode.TwoWay);

        public static readonly BindableProperty RepeatProperty =
            BindableProperty.Create(nameof(Repeat), typeof(RepeatMode), typeof(LottieAnimationView), RepeatMode.Restart);

        public static readonly BindableProperty InfoProperty =
            BindableProperty.Create(nameof(Info), typeof(AnimationInfo), typeof(LottieAnimationView), null, BindingMode.OneWayToSource);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the file path to the Lottie JSON animation.
        /// </summary>
        public string? FilePath
        {
            get => (string?)GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the animation should start playing automatically when loaded.
        /// </summary>
        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty);
            set => SetValue(AutoPlayProperty, value);
        }

        /// <summary>
        /// Gets or sets the number of times the animation should repeat. Use -1 for infinite.
        /// </summary>
        public int RepeatCount
        {
            get => (int)GetValue(RepeatCountProperty);
            set => SetValue(RepeatCountProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the animation is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        /// <summary>
        /// Gets or sets the repeat mode for the animation (Restart or Reverse).
        /// </summary>
        public RepeatMode Repeat
        {
            get => (RepeatMode)GetValue(RepeatProperty);
            set => SetValue(RepeatProperty, value);
        }

        /// <summary>
        /// Gets information about the loaded animation (version, duration, fps, etc).
        /// </summary>
        public AnimationInfo? Info
        {
            get => (AnimationInfo?)GetValue(InfoProperty);
            private set => SetValue(InfoProperty, value);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the animation is stopped.
        /// </summary>
        public event EventHandler? OnStop;

        #endregion

        #region Constructor

        public LottieAnimationView()
        {
            PaintSurface += OnPaintSurface;
        }

        #endregion

        #region Property Changed Callbacks

        private static void OnFilePathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LottieAnimationView view && newValue is string filePath)
            {
                view.LoadAnimationFromPath(filePath);
            }
        }

        private static void OnRepeatCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LottieAnimationView view && newValue is int count)
            {
                view._loopCount = count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts or resumes the animation playback.
        /// </summary>
        public virtual void PlayAnimation()
        {
            if (EnsureVisibleAndEnabled())
            {
                _timer?.Start();
                _watch.Start();
                IsPlaying = true;
            }
            else
            {
                _timer?.Stop();
                _watch.Stop();
            }
        }

        /// <summary>
        /// Stops the animation playback and resets the timer.
        /// </summary>
        public virtual void StopAnimation()
        {
            _loopCount = RepeatCount;
            _timer?.Stop();
            _watch.Reset();
            IsPlaying = false;
            InvalidateSurface();

            OnStop?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Private Methods

        private bool EnsureVisibleAndEnabled()
        {
            return IsVisible && IsEnabled;
        }

        private async void LoadAnimationFromPath(string filePath)
        {
            try
            {
                // 处理MAUI资源路径
                Stream? stream = null;
                
                // 如果是相对路径，从Raw资源中加载
                if (!Path.IsPathRooted(filePath))
                {
                    stream = await FileSystem.OpenAppPackageFileAsync(filePath);
                }
                else
                {
                    // 绝对路径直接读取文件
                    if (File.Exists(filePath))
                    {
                        stream = File.OpenRead(filePath);
                    }
                }

                if (stream != null)
                {
                    // 需要将流复制到内存，因为 SKManagedStream 需要可随机访问的流
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    stream.Dispose();
                    
                    memoryStream.Position = 0;
                    SetAnimation(memoryStream);
                }
                else
                {
                    Debug.WriteLine($"Failed to load animation from path: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading animation from {filePath}: {ex.Message}");
            }
        }

        private void SetAnimation(Stream stream)
        {
            try
            {
            // 创建内存流的副本
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            using var skStream = new SKManagedStream(memoryStream);

            // Use AnimationBuilder with DataUriResourceProvider for base64 support
            using var dataUriProvider = new DataUriResourceProvider(preDecode: true);

            _animation = SkiaSharp.Skottie.Animation
                .CreateBuilder()
                .SetResourceProvider(dataUriProvider)
                .Build(skStream);

            if (_animation != null)
            {
                _animation.Seek(0);
                Info = new AnimationInfo(_animation.Version, _animation.Duration, _animation.Fps, _animation.InPoint, _animation.OutPoint);

                _watch.Reset();
                InitializeTimer();

                if (AutoPlay || IsPlaying)
                {
                PlayAnimation();
                }
            }
            else
            {
                Info = new AnimationInfo(string.Empty, TimeSpan.Zero, 0, 0, 0);
                Debug.WriteLine("Failed to create animation from stream");
            }
            }
            catch (Exception ex)
            {
            Debug.WriteLine($"Error setting animation: {ex.Message}");
            Info = new AnimationInfo(string.Empty, TimeSpan.Zero, 0, 0, 0);
            }
        }

        private void InitializeTimer()
        {
            _timer?.Stop();
            
            if (_animation != null)
            {
                var interval = TimeSpan.FromSeconds(Math.Max(1 / 60.0, 1 / _animation.Fps));
                
                _timer = Dispatcher.CreateTimer();
                _timer.Interval = interval;
                _timer.Tick += (s, e) => InvalidateSurface();
            }
        }

        private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var info = e.Info;

            if (!EnsureVisibleAndEnabled())
            {
                StopAnimation();
                return;
            }

            if (_animation == null) return;

            _animation.SeekFrameTime((float)_watch.Elapsed.TotalSeconds);

            // 处理重复逻辑
            if (_watch.Elapsed.TotalSeconds > _animation.Duration.TotalSeconds)
            {
                if (Repeat == RepeatMode.Restart)
                {
                    if (RepeatCount == LottieDefaults.RepeatCountInfinite)
                    {
                        _watch.Restart();
                    }
                    else if (RepeatCount > 0 && _loopCount > 0)
                    {
                        _loopCount--;
                        _watch.Restart();
                    }
                    else
                    {
                        StopAnimation();
                        return;
                    }
                }
            }

            // 渲染动画，确保适应控件大小
            var rect = new SKRect(0, 0, info.Width, info.Height);
            _animation.Render(canvas, rect);
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _timer?.Stop();
                    _timer = null;
                    _watch.Stop();
                    _animation?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
