namespace MauiApp1.Services;

/// <summary>
/// 机器人传感器事件接口
/// </summary>
public interface IRobotSensorEvents
{
    /// <summary>
    /// 单击事件
    /// </summary>
    event EventHandler? TapDetected;
    
    /// <summary>
    /// 双击事件
    /// </summary>
    event EventHandler? DoubleTapDetected;
    
    /// <summary>
    /// 长按事件
    /// </summary>
    event EventHandler? LongPressDetected;
    
    /// <summary>
    /// 后倾事件
    /// </summary>
    event EventHandler? FallBackwardDetected;
    
    /// <summary>
    /// 前倾事件
    /// </summary>
    event EventHandler? FallForwardDetected;
    
    /// <summary>
    /// 右倾事件
    /// </summary>
    event EventHandler? FallRightDetected;
    
    /// <summary>
    /// 左倾事件
    /// </summary>
    event EventHandler? FallLeftDetected;
    
    /// <summary>
    /// TOF传感器事件
    /// </summary>
    event EventHandler? TofDetected;
}

/// <summary>
/// 机器人控制服务接口
/// </summary>
public interface IRobotControlService : IRobotSensorEvents
{
    /// <summary>
    /// 初始化机器人服务
    /// </summary>
    /// <returns>是否初始化成功</returns>
    Task<bool> InitializeAsync();
    
    /// <summary>
    /// 检查服务是否可用
    /// </summary>
    bool IsServiceAvailable { get; }
    
    // 传感器控制
    /// <summary>
    /// 开启传感器监听
    /// </summary>
    Task StartSensorMonitoringAsync();
    
    /// <summary>
    /// 停止传感器监听
    /// </summary>
    Task StopSensorMonitoringAsync();
    
    // 电机控制
    /// <summary>
    /// 开启电机
    /// </summary>
    Task EnableMotorAsync();
    
    /// <summary>
    /// 关闭电机
    /// </summary>
    Task DisableMotorAsync();
    
    // 动作控制
    /// <summary>
    /// 前进
    /// </summary>
    Task MoveForwardAsync(int speed = 50, int steps = 1);
    
    /// <summary>
    /// 后退
    /// </summary>
    Task MoveBackwardAsync(int speed = 50, int steps = 1);
    
    /// <summary>
    /// 左转
    /// </summary>
    Task TurnLeftAsync(int speed = 50, int steps = 1);
    
    /// <summary>
    /// 右转
    /// </summary>
    Task TurnRightAsync(int speed = 50, int steps = 1);
    
    /// <summary>
    /// 执行自定义动作
    /// </summary>
    Task PerformActionAsync(int actionNumber, int speed = 50, int steps = 1);
    
    // 天线控制
    /// <summary>
    /// 控制天线运动
    /// </summary>
    Task MoveAntennaAsync(int cmd, int step, int speed, int angle);
    
    /// <summary>
    /// 设置天线灯光
    /// </summary>
    Task SetAntennaLightAsync(int color);
    
    /// <summary>
    /// 关闭天线灯光
    /// </summary>
    Task TurnOffAntennaLightAsync();
    
    // 语音和表情
    /// <summary>
    /// 播放TTS语音
    /// </summary>
    Task SpeakAsync(string text);
    
    /// <summary>
    /// 显示表情
    /// </summary>
    Task ShowExpressionAsync(string expression);
    
    /// <summary>
    /// 停止表情
    /// </summary>
    Task StopExpressionAsync();
    
    /// <summary>
    /// 说话并显示表情
    /// </summary>
    Task SpeakWithExpressionAsync(string text, string expression);
    
    // 状态栏控制
    /// <summary>
    /// 控制状态栏
    /// </summary>
    Task ControlStatusBarAsync(string statusBarData);
    
    // 资源清理
    /// <summary>
    /// 清理资源
    /// </summary>
    Task DisposeAsync();
}

/// <summary>
/// 机器人动作命令常量
/// </summary>
public static class RobotActionCommands
{
    public const int MoveForward = 63;
    public const int WalkBackward = 64;
    public const int TurnLeft = 5;
    public const int GoRight = 6;
    public const int TurnToTheLeft = 3;
    public const int TurnToTheRight = 4;
    public const int LeftShakingLeg = 7;
}

/// <summary>
/// 机器人颜色常量
/// </summary>
public static class RobotColors
{
    public const int Red = 0xFF0000;
    public const int Green = 0x00FF00;
    public const int Blue = 0x0000FF;
    public const int Yellow = 0xFFFF00;
    public const int Purple = 0xFF00FF;
    public const int Cyan = 0x00FFFF;
    public const int White = 0xFFFFFF;
    public const int Off = 0x000000;
}

/// <summary>
/// 机器人表情常量
/// </summary>
public static class RobotExpressions
{
    public const string Happy = "happy";
    public const string Sad = "sad";
    public const string Surprised = "surprised";
    public const string Angry = "angry";
    public const string Neutral = "neutral";
    public const string Thinking = "thinking";
    public const string Sleeping = "sleeping";
    public const string Excited = "excited";
}
