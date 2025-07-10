using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MauiApp1.Services;
using Microsoft.Extensions.Logging;

namespace MauiApp1.ViewModels;

/// <summary>
/// 机器人控制主页面ViewModel
/// </summary>
public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly IRobotControlService _robotService;
    private readonly ILogger<MainPageViewModel> _logger;

    private bool _isInitialized = false;
    private bool _isConnecting = false;
    private bool _sensorMonitoring = false;
    private bool _motorEnabled = false;
    private string _statusMessage = "未连接";
    private string _logMessages = "";
    private int _currentLightColor = RobotColors.Off;

    public MainPageViewModel(IRobotControlService robotService, ILogger<MainPageViewModel> logger)
    {
        _robotService = robotService;
        _logger = logger;

        // 初始化命令
        InitializeCommands();

        // 订阅传感器事件
        SubscribeToSensorEvents();
    }

    #region 属性
    public bool IsInitialized
    {
        get => _isInitialized;
        set => SetProperty(ref _isInitialized, value);
    }

    public bool IsConnecting
    {
        get => _isConnecting;
        set => SetProperty(ref _isConnecting, value);
    }

    public bool SensorMonitoring
    {
        get => _sensorMonitoring;
        set => SetProperty(ref _sensorMonitoring, value);
    }

    public bool MotorEnabled
    {
        get => _motorEnabled;
        set 
        { 
            SetProperty(ref _motorEnabled, value);
            OnPropertyChanged(nameof(MotorStatusText));
            OnPropertyChanged(nameof(MotorStatusColor));
        }
    }

    public string MotorStatusText => MotorEnabled ? "⚡ 电机已启用 - 可以使用方向控制" : "🔌 请先启用电机才能控制机器人移动";
    
    public Color MotorStatusColor => MotorEnabled ? Colors.Green : Colors.Orange;

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public string LogMessages
    {
        get => _logMessages;
        set => SetProperty(ref _logMessages, value);
    }

    public bool IsServiceAvailable => _robotService.IsServiceAvailable;

    public string PlatformInfo => DeviceInfo.Platform.ToString();
    #endregion

    #region 命令
    public ICommand InitializeCommand { get; private set; } = null!;
    public ICommand StartSensorCommand { get; private set; } = null!;
    public ICommand StopSensorCommand { get; private set; } = null!;
    public ICommand EnableMotorCommand { get; private set; } = null!;
    public ICommand DisableMotorCommand { get; private set; } = null!;
    
    // 动作命令
    public ICommand MoveForwardCommand { get; private set; } = null!;
    public ICommand MoveBackwardCommand { get; private set; } = null!;
    public ICommand TurnLeftCommand { get; private set; } = null!;
    public ICommand TurnRightCommand { get; private set; } = null!;
    
    // 天线命令
    public ICommand AntennaWaveCommand { get; private set; } = null!;
    public ICommand SetRedLightCommand { get; private set; } = null!;
    public ICommand SetGreenLightCommand { get; private set; } = null!;
    public ICommand SetBlueLightCommand { get; private set; } = null!;
    public ICommand TurnOffLightCommand { get; private set; } = null!;
    
    // 表情和语音命令
    public ICommand ShowHappyCommand { get; private set; } = null!;
    public ICommand ShowSadCommand { get; private set; } = null!;
    public ICommand SpeakHelloCommand { get; private set; } = null!;
    public ICommand PerformDanceCommand { get; private set; } = null!;
    
    public ICommand ClearLogCommand { get; private set; } = null!;
    public ICommand ExitAppCommand { get; private set; } = null!;
    #endregion

    private void InitializeCommands()
    {
        InitializeCommand = new Command(async () => await InitializeRobotAsync(), () => !IsConnecting);
        StartSensorCommand = new Command(async () => await StartSensorMonitoringAsync(), () => IsInitialized && !SensorMonitoring);
        StopSensorCommand = new Command(async () => await StopSensorMonitoringAsync(), () => IsInitialized && SensorMonitoring);
        EnableMotorCommand = new Command(async () => await EnableMotorAsync(), () => IsInitialized && !MotorEnabled);
        DisableMotorCommand = new Command(async () => await DisableMotorAsync(), () => IsInitialized && MotorEnabled);

        MoveForwardCommand = new Command(async () => 
        {
            AddLogMessage($"🎯 执行前进命令 (动作编号: {RobotActionCommands.MoveForward})");
            await _robotService.MoveForwardAsync();
        }, () => MotorEnabled);
        
        MoveBackwardCommand = new Command(async () => 
        {
            AddLogMessage($"🎯 执行后退命令 (动作编号: {RobotActionCommands.WalkBackward})");
            await _robotService.MoveBackwardAsync();
        }, () => MotorEnabled);
        
        TurnLeftCommand = new Command(async () => 
        {
            AddLogMessage($"🎯 执行左转命令 (动作编号: {RobotActionCommands.TurnLeft})");
            await _robotService.TurnLeftAsync();
        }, () => MotorEnabled);
        
        TurnRightCommand = new Command(async () => 
        {
            AddLogMessage($"🎯 执行右转命令 (动作编号: {RobotActionCommands.TurnRight})");
            await _robotService.TurnRightAsync();
        }, () => MotorEnabled);

        AntennaWaveCommand = new Command(async () => await _robotService.MoveAntennaAsync(1, 10, 50, 45), () => IsInitialized);
        SetRedLightCommand = new Command(async () => await SetAntennaLightAsync(RobotColors.Red), () => IsInitialized);
        SetGreenLightCommand = new Command(async () => await SetAntennaLightAsync(RobotColors.Green), () => IsInitialized);
        SetBlueLightCommand = new Command(async () => await SetAntennaLightAsync(RobotColors.Blue), () => IsInitialized);
        TurnOffLightCommand = new Command(async () => await SetAntennaLightAsync(RobotColors.Off), () => IsInitialized);

        ShowHappyCommand = new Command(async () => await _robotService.ShowExpressionAsync(RobotExpressions.Happy), () => IsInitialized);
        ShowSadCommand = new Command(async () => await _robotService.ShowExpressionAsync(RobotExpressions.Sad), () => IsInitialized);
        SpeakHelloCommand = new Command(async () => await _robotService.SpeakAsync("Hello! I'm your robot assistant!"), () => IsInitialized);
        PerformDanceCommand = new Command(async () => await PerformDanceRoutineAsync(), () => MotorEnabled);

        ClearLogCommand = new Command(() => LogMessages = "");
        ExitAppCommand = new Command(async () => await ExitApplicationAsync());
    }

    private void SubscribeToSensorEvents()
    {
        _robotService.TapDetected += (s, e) => AddLogMessage("🖱️ 检测到单击");
        _robotService.DoubleTapDetected += (s, e) => AddLogMessage("🖱️ 检测到双击");
        _robotService.LongPressDetected += (s, e) => AddLogMessage("🖱️ 检测到长按");
        _robotService.FallBackwardDetected += (s, e) => AddLogMessage("⬅️ 检测到后倾");
        _robotService.FallForwardDetected += (s, e) => AddLogMessage("➡️ 检测到前倾");
        _robotService.FallRightDetected += (s, e) => AddLogMessage("⬇️ 检测到右倾");
        _robotService.FallLeftDetected += (s, e) => AddLogMessage("⬆️ 检测到左倾");
        _robotService.TofDetected += (s, e) => AddLogMessage("📡 TOF传感器触发");
    }

    private async Task InitializeRobotAsync()
    {
        try
        {
            IsConnecting = true;
            StatusMessage = "正在连接...";
            AddLogMessage("🔄 开始初始化机器人服务...");

            var success = await _robotService.InitializeAsync();
            
            if (success)
            {
                IsInitialized = true;
                StatusMessage = "已连接";
                AddLogMessage("✅ 机器人服务初始化成功");
            }
            else
            {
                StatusMessage = "连接失败";
                AddLogMessage("❌ 机器人服务初始化失败");
            }
        }
        catch (Exception ex)
        {
            StatusMessage = "连接错误";
            AddLogMessage($"❌ 初始化错误: {ex.Message}");
            _logger.LogError(ex, "初始化机器人服务失败");
        }
        finally
        {
            IsConnecting = false;
            RefreshCanExecute();
        }
    }

    private async Task StartSensorMonitoringAsync()
    {
        try
        {
            AddLogMessage("🎯 启动传感器监听...");
            await _robotService.StartSensorMonitoringAsync();
            SensorMonitoring = true;
            AddLogMessage("✅ 传感器监听已启动");
            RefreshCanExecute();
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 启动传感器失败: {ex.Message}");
            _logger.LogError(ex, "启动传感器监听失败");
        }
    }

    private async Task StopSensorMonitoringAsync()
    {
        try
        {
            AddLogMessage("🛑 停止传感器监听...");
            await _robotService.StopSensorMonitoringAsync();
            SensorMonitoring = false;
            AddLogMessage("✅ 传感器监听已停止");
            RefreshCanExecute();
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 停止传感器失败: {ex.Message}");
            _logger.LogError(ex, "停止传感器监听失败");
        }
    }

    private async Task EnableMotorAsync()
    {
        try
        {
            AddLogMessage("⚡ 启用电机...");
            await _robotService.EnableMotorAsync();
            MotorEnabled = true;
            AddLogMessage("✅ 电机已启用");
            RefreshCanExecute();
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 启用电机失败: {ex.Message}");
            _logger.LogError(ex, "启用电机失败");
        }
    }

    private async Task DisableMotorAsync()
    {
        try
        {
            AddLogMessage("🔌 禁用电机...");
            await _robotService.DisableMotorAsync();
            MotorEnabled = false;
            AddLogMessage("✅ 电机已禁用");
            RefreshCanExecute();
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 禁用电机失败: {ex.Message}");
            _logger.LogError(ex, "禁用电机失败");
        }
    }

    private async Task SetAntennaLightAsync(int color)
    {
        try
        {
            if (color == RobotColors.Off)
            {
                AddLogMessage("💡 关闭天线灯光");
                await _robotService.TurnOffAntennaLightAsync();
            }
            else
            {
                var colorName = color switch
                {
                    RobotColors.Red => "红色",
                    RobotColors.Green => "绿色",
                    RobotColors.Blue => "蓝色",
                    _ => $"0x{color:X6}"
                };
                AddLogMessage($"💡 设置天线灯光为{colorName}");
                await _robotService.SetAntennaLightAsync(color);
            }
            _currentLightColor = color;
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 设置天线灯光失败: {ex.Message}");
            _logger.LogError(ex, "设置天线灯光失败");
        }
    }

    private async Task PerformDanceRoutineAsync()
    {
        try
        {
            AddLogMessage("💃 开始执行舞蹈序列...");
            
            // 显示快乐表情并说话
            await _robotService.SpeakWithExpressionAsync("让我为您跳支舞!", RobotExpressions.Happy);
            
            // 舞蹈动作序列
            await _robotService.TurnLeftAsync();
            await _robotService.SetAntennaLightAsync(RobotColors.Red);
            await Task.Delay(1000);
            
            await _robotService.TurnRightAsync();
            await _robotService.SetAntennaLightAsync(RobotColors.Green);
            await Task.Delay(1000);
            
            await _robotService.MoveAntennaAsync(1, 10, 50, 45);
            await _robotService.SetAntennaLightAsync(RobotColors.Blue);
            await Task.Delay(1000);
            
            await _robotService.SpeakAsync("舞蹈表演结束!");
            await _robotService.TurnOffAntennaLightAsync();
            await _robotService.StopExpressionAsync();
            
            AddLogMessage("✅ 舞蹈序列执行完成");
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 舞蹈序列失败: {ex.Message}");
            _logger.LogError(ex, "执行舞蹈序列失败");
        }
    }

    private void AddLogMessage(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var logEntry = $"[{timestamp}] {message}\n";
        LogMessages += logEntry;
        
        // 限制日志长度
        var lines = LogMessages.Split('\n');
        if (lines.Length > 50)
        {
            LogMessages = string.Join('\n', lines.TakeLast(40));
        }
    }

    private void RefreshCanExecute()
    {
        ((Command)InitializeCommand).ChangeCanExecute();
        ((Command)StartSensorCommand).ChangeCanExecute();
        ((Command)StopSensorCommand).ChangeCanExecute();
        ((Command)EnableMotorCommand).ChangeCanExecute();
        ((Command)DisableMotorCommand).ChangeCanExecute();
        ((Command)MoveForwardCommand).ChangeCanExecute();
        ((Command)MoveBackwardCommand).ChangeCanExecute();
        ((Command)TurnLeftCommand).ChangeCanExecute();
        ((Command)TurnRightCommand).ChangeCanExecute();
        ((Command)AntennaWaveCommand).ChangeCanExecute();
        ((Command)SetRedLightCommand).ChangeCanExecute();
        ((Command)SetGreenLightCommand).ChangeCanExecute();
        ((Command)SetBlueLightCommand).ChangeCanExecute();
        ((Command)TurnOffLightCommand).ChangeCanExecute();
        ((Command)ShowHappyCommand).ChangeCanExecute();
        ((Command)ShowSadCommand).ChangeCanExecute();
        ((Command)SpeakHelloCommand).ChangeCanExecute();
        ((Command)PerformDanceCommand).ChangeCanExecute();
    }

    private async Task ExitApplicationAsync()
    {
        try
        {
            AddLogMessage("🚪 准备退出应用...");
            
            // 清理资源
            if (IsInitialized)
            {
                if (SensorMonitoring)
                {
                    await StopSensorMonitoringAsync();
                }
                
                if (MotorEnabled)
                {
                    await DisableMotorAsync();
                }
                
                AddLogMessage("✅ 资源清理完成");
            }
            
            // 给用户一些时间看到清理完成的消息
            await Task.Delay(1000);
            
            // 退出应用
#if ANDROID
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif IOS
            // iOS不允许程序主动退出，只能返回主屏幕
            AddLogMessage("ℹ️ iOS设备请按Home键退出应用");
#elif WINDOWS
            System.Diagnostics.Process.GetCurrentProcess().Kill();
#else
            Application.Current?.Quit();
#endif
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 退出应用时发生错误: {ex.Message}");
        }
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion
}
