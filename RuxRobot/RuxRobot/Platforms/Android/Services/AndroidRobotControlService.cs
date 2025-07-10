using Android.Content;
using AndroidX.Core.Content;
using Java.Lang;
using MauiApp1.Services;
using Microsoft.Extensions.Logging;
using AndroidApp = Android.App.Application;
using RobotSDK.Core;
using RobotSDK.Messages;
using RobotSDK.Callbacks;
using RobotSDK.Commands;

namespace MauiApp1.Platforms.Android.Services;

/// <summary>
/// 传感器回调实现类
/// 实现ISensorCallback接口，将传感器事件转发到C#事件
/// </summary>
public class SensorCallbackImpl : Java.Lang.Object, ISensorCallback
{
    private readonly Action _onTap;
    private readonly Action _onDoubleTap;
    private readonly Action _onLongPress;
    private readonly Action _onFallBackward;
    private readonly Action _onFallForward;
    private readonly Action _onFallRight;
    private readonly Action _onFallLeft;
    private readonly Action _onTof;

    public SensorCallbackImpl(
        Action onTap,
        Action onDoubleTap,
        Action onLongPress,
        Action onFallBackward,
        Action onFallForward,
        Action onFallRight,
        Action onFallLeft,
        Action onTof)
    {
        _onTap = onTap;
        _onDoubleTap = onDoubleTap;
        _onLongPress = onLongPress;
        _onFallBackward = onFallBackward;
        _onFallForward = onFallForward;
        _onFallRight = onFallRight;
        _onFallLeft = onFallLeft;
        _onTof = onTof;
    }

    public void OnTapResponse() => _onTap?.Invoke();
    public void OnDoubleTapResponse() => _onDoubleTap?.Invoke();
    public void OnLongPressResponse() => _onLongPress?.Invoke();
    public void OnFallBackend() => _onFallBackward?.Invoke();
    public void OnFallForward() => _onFallForward?.Invoke();
    public void OnFallRight() => _onFallRight?.Invoke();
    public void OnFallLeft() => _onFallLeft?.Invoke();
    public void OnTof() => _onTof?.Invoke();
}

/// <summary>
/// Android平台的机器人控制服务实现
/// 使用真实的Robot SDK绑定库
/// </summary>
public class AndroidRobotControlService : IRobotControlService
{
    private readonly ILogger<AndroidRobotControlService> _logger;
    private readonly Context _context;
    private bool _isInitialized = false;
    private bool _sensorMonitoringActive = false;
    private bool _motorEnabled = false;

    // 真实的Robot SDK对象
    private RobotService? _robotService;
    private SensorCallbackImpl? _sensorCallback;

    public AndroidRobotControlService(ILogger<AndroidRobotControlService> logger)
    {
        _logger = logger;
        _context = Platform.CurrentActivity?.ApplicationContext ?? AndroidApp.Context;
    }

    public bool IsServiceAvailable => _isInitialized;

    #region 事件定义
    public event EventHandler? TapDetected;
    public event EventHandler? DoubleTapDetected;
    public event EventHandler? LongPressDetected;
    public event EventHandler? FallBackwardDetected;
    public event EventHandler? FallForwardDetected;
    public event EventHandler? FallRightDetected;
    public event EventHandler? FallLeftDetected;
    public event EventHandler? TofDetected;
    #endregion

    public async Task<bool> InitializeAsync()
    {
        try
        {
            _logger.LogInformation("初始化Android机器人服务...");

            // 获取RobotService实例
            _robotService = RobotService.GetInstance(_context);
            
            if (_robotService == null)
            {
                _logger.LogError("无法获取RobotService实例");
                return false;
            }
            
            // 创建传感器回调
            _sensorCallback = new SensorCallbackImpl(
                onTap: () => TapDetected?.Invoke(this, EventArgs.Empty),
                onDoubleTap: () => DoubleTapDetected?.Invoke(this, EventArgs.Empty),
                onLongPress: () => LongPressDetected?.Invoke(this, EventArgs.Empty),
                onFallBackward: () => FallBackwardDetected?.Invoke(this, EventArgs.Empty),
                onFallForward: () => FallForwardDetected?.Invoke(this, EventArgs.Empty),
                onFallRight: () => FallRightDetected?.Invoke(this, EventArgs.Empty),
                onFallLeft: () => FallLeftDetected?.Invoke(this, EventArgs.Empty),
                onTof: () => TofDetected?.Invoke(this, EventArgs.Empty)
            );

            // 根据官方示例，初始化时自动启用电机
            _logger.LogInformation("自动启用电机...");
            _robotService.RobotOpenMotor();
            await Task.Delay(500);
            _motorEnabled = true;
            _logger.LogInformation("电机已自动启用");

            // 模拟等待连接
            await Task.Delay(500);

            _isInitialized = true;
            _logger.LogInformation("Android机器人服务初始化成功");
            return true;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "初始化Android机器人服务失败");
            return false;
        }
    }

    #region 传感器控制
    public async Task StartSensorMonitoringAsync()
    {
        try
        {
            if (!_isInitialized || _robotService == null)
            {
                _logger.LogWarning("服务未初始化，无法启动传感器监听");
                return;
            }

            _logger.LogInformation("启动传感器监听...");

            // 使用真实的Robot SDK API
            _robotService.RobotRegisterSensorCallback(_sensorCallback);
            _robotService.RobotOpenSensor();

            await Task.Delay(500);
            _sensorMonitoringActive = true;
            _logger.LogInformation("传感器监听已启动");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "启动传感器监听失败");
        }
    }

    public async Task StopSensorMonitoringAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation("停止传感器监听...");

            // 使用真实的Robot SDK API
            _robotService.RobotUnregisterSensor();
            _robotService.RobotCloseSensor();

            await Task.Delay(200);
            _sensorMonitoringActive = false;
            _logger.LogInformation("传感器监听已停止");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "停止传感器监听失败");
        }
    }
    #endregion

    #region 电机控制
    public async Task EnableMotorAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation("启用电机...");

            // 使用真实的Robot SDK API
            _robotService.RobotOpenMotor();

            await Task.Delay(500);
            _motorEnabled = true;
            _logger.LogInformation("电机已启用");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "启用电机失败");
        }
    }

    public async Task DisableMotorAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation("禁用电机...");

            // 使用真实的Robot SDK API
            _robotService.RobotCloseMotor();

            await Task.Delay(200);
            _motorEnabled = false;
            _logger.LogInformation("电机已禁用");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "禁用电机失败");
        }
    }
    #endregion

    #region 动作控制
    public async Task MoveForwardAsync(int speed = 3, int steps = 3)
    {
        _logger.LogInformation($"请求前进动作: 编号={RobotActionCommands.MoveForward}, 速度={speed}, 步数={steps}");
        await PerformActionAsync(RobotActionCommands.MoveForward, speed, steps);
    }

    public async Task MoveBackwardAsync(int speed = 3, int steps = 3)
    {
        _logger.LogInformation($"请求后退动作: 编号={RobotActionCommands.WalkBackward}, 速度={speed}, 步数={steps}");
        await PerformActionAsync(RobotActionCommands.WalkBackward, speed, steps);
    }

    public async Task TurnLeftAsync(int speed = 3, int steps = 3)
    {
        _logger.LogInformation($"请求左转动作: 编号={RobotActionCommands.TurnLeft}, 速度={speed}, 步数={steps}");
        await PerformActionAsync(RobotActionCommands.TurnLeft, speed, steps);
    }

    public async Task TurnRightAsync(int speed = 3, int steps = 3)
    {
        _logger.LogInformation($"请求右转动作: 编号={RobotActionCommands.TurnRight}, 速度={speed}, 步数={steps}");
        await PerformActionAsync(RobotActionCommands.TurnRight, speed, steps);
    }

    public async Task PerformActionAsync(int actionNumber, int speed = 3, int steps = 3)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化，无法执行动作");
                return;
            }

            if (!_motorEnabled)
            {
                _logger.LogWarning("电机未启用，无法执行动作。请先调用EnableMotorAsync()启用电机");
                return;
            }

            _logger.LogInformation($"执行动作: {actionNumber}, 速度: {speed}, 步数: {steps}");

            // 使用正确的参数顺序！
            // 根据官方SDK文档：mMessage.set(number, speed, stepNum)
            var actionMessage = new ActionMessage();
            
            // 正确的参数顺序：Set(actionNumber, speed, steps)
            _logger.LogInformation($"使用参数顺序: Set({actionNumber}, {speed}, {steps})");
            actionMessage.Set(actionNumber, speed, steps);
            _robotService.RobotActionCommand(actionMessage);

            // 等待动作完成 - 根据动作类型调整等待时间
            int waitTime = actionNumber switch
            {
                63 or 64 => 1500, // 前进后退需要更长时间
                5 or 6 => 1000,   // 转向动作
                3 or 4 => 1000,   // 其他转向动作
                _ => 800           // 其他动作
            };
            
            await Task.Delay(waitTime);
            _logger.LogInformation($"动作 {actionNumber} 执行完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"执行动作 {actionNumber} 失败");
        }
    }
    #endregion

    #region 天线控制
    public async Task MoveAntennaAsync(int cmd, int step, int speed, int angle)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"控制天线运动: cmd={cmd}, step={step}, speed={speed}, angle={angle}");

            // 使用真实的Robot SDK
            var antennaMessage = new AntennaMessage();
            antennaMessage.Set(cmd, step, speed, angle);
            _robotService.RobotAntennaMotion(antennaMessage);

            await Task.Delay(800);
            _logger.LogInformation("天线运动完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "控制天线运动失败");
        }
    }

    public async Task SetAntennaLightAsync(int color)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"设置天线灯光颜色: 0x{color:X6}");

            // 使用真实的Robot SDK
            var lightMessage = new AntennaLightMessage();
            lightMessage.Set(color);
            _robotService.RobotAntennaLight(lightMessage);

            await Task.Delay(200);
            _logger.LogInformation("天线灯光设置完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "设置天线灯光失败");
        }
    }

    public async Task TurnOffAntennaLightAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation("关闭天线灯光");

            // 使用真实的Robot SDK
            _robotService.RobotCloseAntennaLight();

            await Task.Delay(200);
            _logger.LogInformation("天线灯光已关闭");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "关闭天线灯光失败");
        }
    }
    #endregion

    #region 语音和表情
    public async Task SpeakAsync(string text)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"播放TTS: {text}");

            // 使用真实的Robot SDK
            _robotService.RobotPlayTTs(text);

            // 模拟TTS播放时间
            await Task.Delay(text.Length * 100);
            _logger.LogInformation("TTS播放完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "播放TTS失败");
        }
    }

    public async Task ShowExpressionAsync(string expression)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"显示表情: {expression}");

            // 使用真实的Robot SDK
            _robotService.RobotStartExpression(expression);

            await Task.Delay(300);
            _logger.LogInformation($"表情 {expression} 显示完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "显示表情失败");
        }
    }

    public async Task StopExpressionAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation("停止表情");

            // 使用真实的Robot SDK
            _robotService.RobotStopExpression();

            await Task.Delay(200);
            _logger.LogInformation("表情已停止");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "停止表情失败");
        }
    }

    public async Task SpeakWithExpressionAsync(string text, string expression)
    {
        try
        {
            // 先显示表情
            await ShowExpressionAsync(expression);
            
            // 再播放语音
            await SpeakAsync(text);
            
            // 延迟后停止表情
            await Task.Delay(500);
            await StopExpressionAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "播放语音和表情失败");
        }
    }
    #endregion

    #region 状态栏控制
    public async Task ControlStatusBarAsync(string statusBarData)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"控制状态栏: {statusBarData}");

            // 使用真实的Robot SDK
            _robotService.RobotControlStatusBar(statusBarData);

            await Task.Delay(200);
            _logger.LogInformation("状态栏控制完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "控制状态栏失败");
        }
    }
    #endregion

    #region 音效控制
    /// <summary>
    /// 播放内置音效
    /// </summary>
    /// <param name="soundId">音效ID (如: "a0001", "a0005"等)</param>
    public async Task PlaySoundAsync(string soundId)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"播放音效: {soundId}");

            // 使用真实的Robot SDK
            _robotService.RobotControlSound(soundId);

            await Task.Delay(500);
            _logger.LogInformation($"音效 {soundId} 播放完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"播放音效 {soundId} 失败");
        }
    }

    /// <summary>
    /// 发送长连接命令(如舞蹈、唱歌)
    /// </summary>
    /// <param name="command">命令类型 (如: "speechDance", "speechMusic")</param>
    /// <param name="data">命令数据</param>
    public async Task SendLongCommandAsync(string command, string data)
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化");
                return;
            }

            _logger.LogInformation($"发送长连接命令: {command}, 数据: {data}");

            // 使用真实的Robot SDK
            _robotService.SendLongCommand(command, data);

            await Task.Delay(300);
            _logger.LogInformation($"长连接命令 {command} 发送完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"发送长连接命令 {command} 失败");
        }
    }
    #endregion

    #region 资源清理
    public async Task DisposeAsync()
    {
        try
        {
            _logger.LogInformation("清理机器人服务资源...");

            await StopSensorMonitoringAsync();
            await DisableMotorAsync();
            await TurnOffAntennaLightAsync();
            await StopExpressionAsync();

            // 使用真实的Robot SDK
            _robotService?.UnbindService();

            _isInitialized = false;
            _logger.LogInformation("机器人服务资源清理完成");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "清理机器人服务资源失败");
        }
    }
    #endregion

    #region 调试测试
    /// <summary>
    /// 调试方法：尝试多种动作编号来找出正确的前进动作
    /// </summary>
    public async Task DebugTestForwardActionsAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化，无法执行调试测试");
                return;
            }

            if (!_motorEnabled)
            {
                _logger.LogWarning("电机未启用，无法执行调试测试");
                return;
            }

            _logger.LogInformation("=== 开始调试测试前进动作 ===");
            
            // 尝试不同的动作编号
            int[] testActions = { 1, 2, 3, 4, 5, 6, 63, 64, 65, 66 };
            
            foreach (int actionNumber in testActions)
            {
                _logger.LogInformation($"测试动作编号: {actionNumber}");
                
                var actionMessage = new ActionMessage();
                actionMessage.Set(actionNumber, 3, 1);  // 使用正确的参数顺序和默认值
                _robotService.RobotActionCommand(actionMessage);
                
                // 等待观察效果
                await Task.Delay(3000);
                
                _logger.LogInformation($"动作编号 {actionNumber} 测试完成，请观察机器人是否有反应");
            }
            
            _logger.LogInformation("=== 调试测试完成 ===");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "调试测试失败");
        }
    }

    /// <summary>
    /// 调试方法：尝试不同的参数组合
    /// </summary>
    public async Task DebugTestParameterCombinationsAsync()
    {
        try
        {
            if (_robotService == null)
            {
                _logger.LogWarning("RobotService未初始化，无法执行参数测试");
                return;
            }

            if (!_motorEnabled)
            {
                _logger.LogWarning("电机未启用，无法执行参数测试");
                return;
            }

            _logger.LogInformation("=== 开始测试参数组合 ===");
            
            // 使用动作编号63，尝试不同的参数组合
            int actionNumber = 63;
            
            // 组合1: Set(actionNumber, speed, steps) - 正确的官方文档顺序
            _logger.LogInformation($"测试组合1: Set({actionNumber}, 3, 1)");
            var msg1 = new ActionMessage();
            msg1.Set(actionNumber, 3, 1);
            _robotService.RobotActionCommand(msg1);
            await Task.Delay(3000);
            
            // 组合2: Set(speed, steps, actionNumber) - 错误的参数顺序
            _logger.LogInformation($"测试组合2: Set(3, 1, {actionNumber})");
            var msg2 = new ActionMessage();
            msg2.Set(3, 1, actionNumber);
            _robotService.RobotActionCommand(msg2);
            await Task.Delay(3000);
            
            // 组合3: Set(steps, actionNumber, speed) - 错误的参数顺序
            _logger.LogInformation($"测试组合3: Set(1, {actionNumber}, 3)");
            var msg3 = new ActionMessage();
            msg3.Set(1, actionNumber, 3);
            _robotService.RobotActionCommand(msg3);
            await Task.Delay(3000);
            
            // 组合4: 使用更高的速度值
            _logger.LogInformation($"测试组合4: Set({actionNumber}, 6, 2)");
            var msg4 = new ActionMessage();
            msg4.Set(actionNumber, 6, 2);
            _robotService.RobotActionCommand(msg4);
            await Task.Delay(3000);
            
            _logger.LogInformation("=== 参数组合测试完成 ===");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "参数组合测试失败");
        }
    }
    #endregion
}