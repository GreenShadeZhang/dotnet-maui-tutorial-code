using MauiApp1.Services;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services;

/// <summary>
/// 默认的机器人控制服务实现（用于非Android平台）
/// </summary>
public class DefaultRobotControlService : IRobotControlService
{
    private readonly ILogger<DefaultRobotControlService> _logger;

    public DefaultRobotControlService(ILogger<DefaultRobotControlService> logger)
    {
        _logger = logger;
    }

    public bool IsServiceAvailable => false;

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

    public Task<bool> InitializeAsync()
    {
        _logger.LogWarning("机器人控制服务仅在Android平台可用");
        return Task.FromResult(false);
    }

    public Task StartSensorMonitoringAsync()
    {
        _logger.LogWarning("传感器监听仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task StopSensorMonitoringAsync()
    {
        _logger.LogWarning("传感器监听仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task EnableMotorAsync()
    {
        _logger.LogWarning("电机控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task DisableMotorAsync()
    {
        _logger.LogWarning("电机控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task MoveForwardAsync(int speed = 50, int steps = 1)
    {
        _logger.LogWarning("动作控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task MoveBackwardAsync(int speed = 50, int steps = 1)
    {
        _logger.LogWarning("动作控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task TurnLeftAsync(int speed = 50, int steps = 1)
    {
        _logger.LogWarning("动作控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task TurnRightAsync(int speed = 50, int steps = 1)
    {
        _logger.LogWarning("动作控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task PerformActionAsync(int actionNumber, int speed = 50, int steps = 1)
    {
        _logger.LogWarning("动作控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task MoveAntennaAsync(int cmd, int step, int speed, int angle)
    {
        _logger.LogWarning("天线控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task SetAntennaLightAsync(int color)
    {
        _logger.LogWarning("天线灯光控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task TurnOffAntennaLightAsync()
    {
        _logger.LogWarning("天线灯光控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task SpeakAsync(string text)
    {
        _logger.LogWarning("语音播放仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task ShowExpressionAsync(string expression)
    {
        _logger.LogWarning("表情控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task StopExpressionAsync()
    {
        _logger.LogWarning("表情控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task SpeakWithExpressionAsync(string text, string expression)
    {
        _logger.LogWarning("语音和表情控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task ControlStatusBarAsync(string statusBarData)
    {
        _logger.LogWarning("状态栏控制仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task PlaySoundAsync(string soundId)
    {
        _logger.LogWarning("音效播放仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task SendLongCommandAsync(string command, string data)
    {
        _logger.LogWarning("长连接命令仅在Android平台可用");
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _logger.LogInformation("默认机器人服务清理完成");
        return Task.CompletedTask;
    }
}
