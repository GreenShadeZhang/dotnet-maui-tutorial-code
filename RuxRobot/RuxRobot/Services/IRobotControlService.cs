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
    
    /// <summary>
    /// 播放内置音效
    /// </summary>
    /// <param name="soundId">音效ID (如: "a0001", "a0005"等)</param>
    Task PlaySoundAsync(string soundId);
    
    /// <summary>
    /// 发送长连接命令
    /// </summary>
    /// <param name="command">命令类型 (如: "speechDance", "speechMusic")</param>
    /// <param name="data">命令数据</param>
    Task SendLongCommandAsync(string command, string data);
    
    // 资源清理
    /// <summary>
    /// 清理资源
    /// </summary>
    Task DisposeAsync();

    /// <summary>
    /// 调试方法：尝试多种动作编号来找出正确的前进动作
    /// </summary>
    Task DebugTestForwardActionsAsync();

    /// <summary>
    /// 调试方法：尝试不同的参数组合
    /// </summary>
    Task DebugTestParameterCombinationsAsync();
}

/// <summary>
/// 机器人动作命令常量
/// 根据官方SDK文档：https://cn.letianpai.com/?p=5915
/// </summary>
public static class RobotActionCommands
{
    // 基础移动动作 (根据官方文档修正)
    public const int MoveForward = 63;      // 前进动作2 (推荐使用)
    public const int WalkBackward = 64;     // 后退动作2 (推荐使用)
    public const int TurnLeft = 3;          // 左转
    public const int TurnRight = 4;         // 右转
    
    // 螃蟹步动作 (根据官方文档)
    public const int CrabStepLeft = 5;      // 螃蟹步左走
    public const int CrabStepRight = 6;     // 螃蟹步右走
    
    // 抖腿动作
    public const int LeftShakingLeg = 7;    // 左抖腿
    public const int RightShakingLeg = 8;   // 右抖腿
    
    // 抖脚动作
    public const int LeftShakingFoot = 9;   // 左抖脚
    public const int RightShakingFoot = 10; // 右抖脚
    
    // 翘脚动作
    public const int LeftFootUp = 11;       // 左翘脚
    public const int RightFootUp = 12;      // 右翘脚
    
    // 倾身动作
    public const int LeanLeft = 13;         // 左倾身
    public const int LeanRight = 14;        // 右倾身
    
    // 跺脚动作
    public const int StompLeft = 15;        // 左跺脚
    public const int StompRight = 16;       // 右跺脚
    
    // 身体摆动
    public const int BodyUpDown = 17;       // 身体上下摆动
    public const int BodyLeftRight = 18;    // 身体左右摆动
    
    // 头部动作
    public const int HeadShake = 19;        // 左右摇头
    public const int Attention = 20;        // 稍息
    
    // 原地转身
    public const int TurnInPlaceLeft = 21;  // 原地左转
    public const int TurnInPlaceRight = 22; // 原地右转
    
    // 组合动作
    public const int DoubleShakeFoot = 23;  // 双抖脚
    public const int MicroShake = 24;       // 微抖
    public const int MicroTurnLeft = 25;    // 微左转
    public const int MicroTurnRight = 26;   // 微右转
    public const int Sway = 27;             // 摇摆
    public const int ShakeHead = 28;        // 摇头
    
    // 舞蹈动作
    public const int DanceMove = 80;        // 舞蹈扭动
    
    // 备用动作编号（用于调试测试）
    public const int MoveForward1 = 1;      // 向前走 (备用编号)
    public const int WalkBackward1 = 2;     // 向后走 (备用编号)
}

/// <summary>
/// 机器人颜色常量
/// </summary>
public static class RobotColors
{
    public const int Red = 1;
    public const int Green = 2;
    public const int Blue = 3;
    public const int Orange = 4;
    public const int White = 5;
    public const int Yellow = 6;
    public const int Purple = 7;
    public const int Cyan = 8;
    public const int Black = 9;
    public const int Off = 9;
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

/// <summary>
/// 机器人音效常量
/// </summary>
public static class RobotSounds
{
    public const string Beep = "a0001";
    public const string Hammer = "a0005";
    public const string Laser = "a0007";
    public const string Pulse = "a0010";
    public const string Electromagnetic = "a0011";
    public const string Charging = "a0025";
    public const string Click = "a0028";
    public const string Alarm = "a0029";
    public const string Comfortable = "a0030";
    public const string Confused = "a0031";
    public const string Hahaha = "a0032";
    public const string Dizzy = "a0034";
    public const string Tired = "a0036";
    public const string Afraid = "a0037";
    public const string Surprised = "a0039";
    public const string Complete = "a0040";
    public const string IAmLetianpai = "a0054";
    public const string LowBattery = "a0055";
    public const string Warning = "a0056";
    public const string NoBattery = "a0057";
    public const string Error = "a0059";
    public const string Breathing = "a0064";
    public const string Wow = "a0067";
    public const string Panic = "a0070";
    public const string Snoring = "a0077";
    public const string Response = "a0084";
    public const string Sad = "a0086";
    public const string Shutdown = "a0088";
    public const string Shy = "a0090";
    public const string Startup = "a0093";
    public const string Happy = "a0100";
    public const string Embarrassed = "a0107";
    public const string Bell = "a0108";
    public const string Proud = "a0115";
    public const string Horn = "a0118";
    public const string Doorbell = "a0121";
    public const string Typing = "a0125";
    public const string NewMessage = "a0126";
    public const string PhoneRing = "a0132";
    public const string Lost = "a0133";
}

/// <summary>
/// 机器人长连接命令常量
/// </summary>
public static class RobotLongCommands
{
    public const string SpeechDance = "speechDance";
    public const string SpeechMusic = "speechMusic";
}
