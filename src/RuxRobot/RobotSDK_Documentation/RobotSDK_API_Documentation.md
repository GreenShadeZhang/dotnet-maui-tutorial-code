# Robot SDK API 接口文档

## 概述

Robot SDK 是一个用于控制机器人的Android SDK，提供了机器人动作控制、传感器管理、表情控制、语音播放等功能。

## 核心架构

### 主要包结构
- `com.leitianpai.robotsdk` - SDK核心包
- `com.renhejia.robot.letianpaiservice` - 机器人服务接口
- `com.letianpai.robot.letianpaiservice` - 回调接口

## 主要类和接口

### 1. RobotService (核心服务类)

`com.leitianpai.robotsdk.RobotService`

这是SDK的核心服务类，提供单例模式访问机器人的所有功能。

#### 主要方法

##### 初始化和连接
- `getInstance(Context context)` - 获取RobotService单例实例
- `getAIDL()` - 获取AIDL服务接口
- `unbindService()` - 解绑服务

##### 传感器控制
- `robotRegisterSensorCallback(SensorCallback callback)` - 注册传感器回调
- `robotUnregisterSensor()` - 注销传感器回调
- `robotOpenSensor()` - 开启传感器
- `robotCloseSensor()` - 关闭传感器

##### 电机控制
- `robotOpenMotor()` - 开启电机
- `robotCloseMotor()` - 关闭电机

##### 动作控制
- `robotActionCommand(ActionMessage message)` - 发送动作命令
- `robotAntennaMotion(AntennaMessage message)` - 控制天线运动
- `robotAntennaLight(AntennaLightMessage message)` - 控制天线灯光
- `robotCloseAntennaLight()` - 关闭天线灯光

##### 音频控制
- `robotControlSound(String soundData)` - 控制声音播放
- `robotPlayTTs(String text)` - 播放TTS语音

##### 表情控制
- `robotStartExpression(String expression)` - 开始播放表情
- `robotChangeExpression(String expression)` - 切换表情
- `robotStopExpression()` - 停止表情播放

##### 状态栏控制
- `robotControlStatusBar(String statusBarData)` - 控制状态栏

##### 长连接命令
- `sendLongCommand(String command, String data)` - 发送长连接命令
- `sendExpressionCommand(String command)` - 发送表情命令

### 2. ILetianpaiService (AIDL服务接口)

`com.renhejia.robot.letianpaiservice.ILetianpaiService`

机器人服务的AIDL接口，提供所有机器人功能的底层访问。

#### 主要方法

##### 基础功能
- `int getRobotStatus()` - 获取机器人状态
- `setRobotStatus(int status)` - 设置机器人状态
- `setCommand(LtpCommand command)` - 设置命令

##### 回调管理
- `registerCallback(LtpCommandCallback callback)` - 注册命令回调
- `unregisterCallback(LtpCommandCallback callback)` - 注销命令回调

##### 长连接管理
- `setLongConnectCommand(String command, String data)` - 设置长连接命令
- `registerLCCallback(LtpLongConnectCallback callback)` - 注册长连接回调
- `unregisterLCCallback(LtpLongConnectCallback callback)` - 注销长连接回调

##### MCU命令管理
- `setMcuCommand(String command, String data)` - 设置MCU命令
- `registerMcuCmdCallback(LtpMcuCommandCallback callback)` - 注册MCU命令回调
- `unregisterMcuCmdCallback(LtpMcuCommandCallback callback)` - 注销MCU命令回调

##### 音频效果管理
- `setAudioEffect(String command, String data)` - 设置音频效果
- `registerAudioEffectCallback(LtpAudioEffectCallback callback)` - 注册音频效果回调
- `unregisterAudioEffectCallback(LtpAudioEffectCallback callback)` - 注销音频效果回调

##### 表情管理
- `setExpression(String command, String data)` - 设置表情
- `registerExpressionCallback(LtpExpressionCallback callback)` - 注册表情回调
- `unregisterExpressionCallback(LtpExpressionCallback callback)` - 注销表情回调

##### 应用命令管理
- `setAppCmd(String command, String data)` - 设置应用命令
- `registerAppCmdCallback(LtpAppCmdCallback callback)` - 注册应用命令回调
- `unregisterAppCmdCallback(LtpAppCmdCallback callback)` - 注销应用命令回调

##### 机器人状态管理
- `setRobotStatusCmd(String command, String data)` - 设置机器人状态命令
- `registerRobotStatusCallback(LtpRobotStatusCallback callback)` - 注册机器人状态回调
- `unregisterRobotStatusCallback(LtpRobotStatusCallback callback)` - 注销机器人状态回调

##### TTS管理
- `setTTS(String command, String data)` - 设置TTS
- `registerTTSCallback(LtpTTSCallback callback)` - 注册TTS回调
- `unregisterTTSCallback(LtpTTSCallback callback)` - 注销TTS回调

##### 语音管理
- `setSpeechCmd(String command, String data)` - 设置语音命令
- `registerSpeechCallback(LtpSpeechCallback callback)` - 注册语音回调
- `unregisterSpeechCallback(LtpSpeechCallback callback)` - 注销语音回调

##### 传感器响应管理
- `setSensorResponse(String command, String data)` - 设置传感器响应
- `registerSensorResponseCallback(LtpSensorResponseCallback callback)` - 注册传感器响应回调
- `unregisterSensorResponseCallback(LtpSensorResponseCallback callback)` - 注销传感器响应回调

##### 小米命令管理
- `setMiCmd(String command, String data)` - 设置小米命令
- `registerMiCmdResponseCallback(LtpMiCmdCallback callback)` - 注册小米命令响应回调
- `unregisterMiCmdResponseCallback(LtpMiCmdCallback callback)` - 注销小米命令响应回调

##### 识别命令管理
- `setIdentifyCmd(String command, String data)` - 设置识别命令
- `registerIdentifyCmdCallback(LtpIdentifyCmdCallback callback)` - 注册识别命令回调
- `unregisterIdentifyCmdCallback(LtpIdentifyCmdCallback callback)` - 注销识别命令回调

##### 蓝牙管理
- `setBleCmd(String command, String data, boolean flag)` - 设置蓝牙命令
- `registerBleCmdCallback(LtpBleCallback callback)` - 注册蓝牙命令回调
- `unregisterBleCmdCallback(LtpBleCallback callback)` - 注销蓝牙命令回调
- `setBleResponse(String command, String data)` - 设置蓝牙响应
- `registerBleResponseCallback(LtpBleResponseCallback callback)` - 注册蓝牙响应回调
- `unregisterBleResponseCmdCallback(LtpBleResponseCallback callback)` - 注销蓝牙响应回调

## 消息类

### 1. ActionMessage (动作消息)

`com.leitianpai.robotsdk.message.ActionMessage`

用于发送机器人动作控制命令。

#### 属性
- `String motion` - 动作类型
- `int number` - 动作编号
- `int speed` - 动作速度
- `String desc` - 动作描述
- `Integer id` - 动作ID
- `int stepNum` - 步数

#### 构造方法
- `ActionMessage()` - 默认构造方法
- `ActionMessage(int number, int speed, int stepNum)` - 带参数构造方法

#### 主要方法
- `set(int number, int speed, int stepNum)` - 设置动作参数
- `setNumber(int number)` - 设置动作编号
- `setSpeed(int speed)` - 设置动作速度
- `setStepNum(int stepNum)` - 设置步数
- `setId(Integer id)` - 设置动作ID
- `setDesc(String desc)` - 设置动作描述
- `setMotion(String motion)` - 设置动作类型

### 2. AntennaMessage (天线消息)

`com.leitianpai.robotsdk.message.AntennaMessage`

用于控制机器人天线运动。

#### 属性
- `String antenna_motion` - 天线运动
- `String antenna_motion_name` - 天线运动名称
- `int cmd` - 命令
- `int step` - 步数
- `int speed` - 速度
- `int angle` - 角度

#### 主要方法
- `set(int cmd, int step, int speed, int angle)` - 设置天线运动参数
- `getCmd()` - 获取命令
- `getStep()` - 获取步数
- `getSpeed()` - 获取速度
- `getAngle()` - 获取角度

### 3. AntennaLightMessage (天线灯光消息)

`com.leitianpai.robotsdk.message.AntennaLightMessage`

用于控制机器人天线灯光。

#### 属性
- `String antenna_light` - 天线灯光
- `int antenna_light_color` - 天线灯光颜色

#### 主要方法
- `set(int color)` - 设置天线灯光颜色

### 4. LtpCommand (命令类)

`com.renhejia.robot.letianpaiservice.LtpCommand`

实现Parcelable接口的命令类，用于进程间通信。

#### 属性
- `String command` - 命令字符串
- `String data` - 命令数据

#### 构造方法
- `LtpCommand()` - 默认构造方法
- `LtpCommand(String command, String data)` - 带参数构造方法

#### 主要方法
- `getCommand()` - 获取命令
- `setCommand(String command)` - 设置命令
- `getData()` - 获取数据
- `setData(String data)` - 设置数据

## 回调接口

### 1. SensorCallback (传感器回调)

`com.leitianpai.robotsdk.callback.SensorCallback`

传感器事件回调接口。

#### 方法
- `onTapResponse()` - 单击响应
- `onDoubleTapResponse()` - 双击响应
- `onLongPressResponse()` - 长按响应
- `onFallBackend()` - 后倾响应
- `onFallForward()` - 前倾响应
- `onFallRight()` - 右倾响应
- `onFallLeft()` - 左倾响应
- `onTof()` - TOF传感器响应

## 常量定义

### RobotRemoteConsts (机器人遥控常量)

`com.leitianpai.robotsdk.commandlib.RobotRemoteConsts`

#### 命令类型常量
- `COMMAND_TYPE_MOTION = "controlMotion"` - 动作控制命令
- `POWER_CONTROL = "powerControl"` - 电源控制
- `POWER_DOWN = "powerDown"` - 关机
- `COMMAND_TYPE_ANTENNA_MOTION = "controlAntennaMotion"` - 天线运动控制
- `COMMAND_TYPE_ANTENNA_LIGHT = "controlAntennaLight"` - 天线灯光控制
- `COMMAND_TYPE_SHOW_TIME = "showTIme"` - 显示时间
- `COMMAND_TYPE_FACE = "controlFace"` - 表情控制
- `COMMAND_TYPE_SOUND = "controlSound"` - 声音控制

#### 传感器命令常量
- `COMMAND_CONTROL_TAP = "controlTap"` - 单击控制
- `COMMAND_CONTROL_DOUBLE_TAP = "controlDoubleTap"` - 双击控制
- `COMMAND_CONTROL_LONG_PRESS_TAP = "controlLongPressTap"` - 长按控制
- `COMMAND_FALL_BACKEND = "fallBackend"` - 后倾
- `COMMAND_FALL_FORWARD = "fallForward"` - 前倾
- `COMMAND_FALL_RIGHT = "fallRight"` - 右倾
- `COMMAND_FALL_LEFT = "fallLeft"` - 左倾
- `COMMAND_TOF = "tof"` - TOF传感器

#### 服务常量
- `LTP_PACKAGE_NAME = "com.renhejia.robot.letianpaiservice"` - 服务包名
- `ACTION_LETIANPAI = "android.intent.action.LETIANPAI"` - 服务Action

#### 动作命令常量
- `MOVE_FORWARD = "63"` - 前进
- `WALK_BACKWARD = "64"` - 后退
- `TURN_LEFT = "5"` - 左转
- `GO_RIGHT = "6"` - 右转
- `TURN_TO_THE_LEFT = "3"` - 向左转
- `TURN_TO_THE_RIGHT = "4"` - 向右转
- `LEFT_SHAKING_LEG = "7"` - 左摇腿

## 使用示例

### 1. 初始化SDK

```java
// 获取RobotService实例
RobotService robotService = RobotService.getInstance(context);
```

### 2. 注册传感器回调

```java
SensorCallback sensorCallback = new SensorCallback() {
    @Override
    public void onTapResponse() {
        // 处理单击事件
    }
    
    @Override
    public void onDoubleTapResponse() {
        // 处理双击事件
    }
    
    @Override
    public void onLongPressResponse() {
        // 处理长按事件
    }
    
    @Override
    public void onFallBackend() {
        // 处理后倾事件
    }
    
    @Override
    public void onFallForward() {
        // 处理前倾事件
    }
    
    @Override
    public void onFallRight() {
        // 处理右倾事件
    }
    
    @Override
    public void onFallLeft() {
        // 处理左倾事件
    }
    
    @Override
    public void onTof() {
        // 处理TOF传感器事件
    }
};

// 注册传感器回调
robotService.robotRegisterSensorCallback(sensorCallback);

// 开启传感器
robotService.robotOpenSensor();
```

### 3. 控制机器人动作

```java
// 创建动作消息
ActionMessage actionMessage = new ActionMessage();
actionMessage.setNumber(63); // 前进动作
actionMessage.setSpeed(50);  // 速度
actionMessage.setStepNum(1); // 步数

// 发送动作命令
robotService.robotActionCommand(actionMessage);
```

### 4. 控制天线运动

```java
// 创建天线运动消息
AntennaMessage antennaMessage = new AntennaMessage();
antennaMessage.set(1, 10, 50, 90); // cmd, step, speed, angle

// 发送天线运动命令
robotService.robotAntennaMotion(antennaMessage);
```

### 5. 控制天线灯光

```java
// 创建天线灯光消息
AntennaLightMessage lightMessage = new AntennaLightMessage();
lightMessage.set(0xFF0000); // 红色

// 发送天线灯光命令
robotService.robotAntennaLight(lightMessage);
```

### 6. 播放TTS语音

```java
// 播放TTS语音
robotService.robotPlayTTs("Hello, I am a robot!");
```

### 7. 控制表情

```java
// 开始播放表情
robotService.robotStartExpression("happy");

// 切换表情
robotService.robotChangeExpression("sad");

// 停止表情
robotService.robotStopExpression();
```

### 8. 资源清理

```java
// 注销传感器
robotService.robotUnregisterSensor();

// 关闭传感器
robotService.robotCloseSensor();

// 解绑服务
robotService.unbindService();
```

## 注意事项

1. **权限要求**: 使用SDK需要相应的Android权限
2. **服务连接**: 需要确保机器人服务正在运行
3. **异常处理**: 所有AIDL调用都可能抛出RemoteException，需要适当处理
4. **线程安全**: 回调方法可能在非主线程中调用，需要注意线程安全
5. **资源管理**: 使用完毕后应及时注销回调和解绑服务

## 版本信息

- SDK版本: 2.5
- 目标Android版本: 根据项目配置
- 最小支持版本: 需要查看具体要求

---

*本文档基于反编译的class文件生成，可能存在某些细节上的不准确，建议结合官方文档和源码进行开发。*
