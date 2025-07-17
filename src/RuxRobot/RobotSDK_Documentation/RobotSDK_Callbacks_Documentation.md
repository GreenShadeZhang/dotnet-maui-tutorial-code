# Robot SDK 回调接口详细文档

## 概述

Robot SDK 提供了多种回调接口，用于接收机器人的各种状态变化和事件通知。所有AIDL回调接口都继承自`android.os.IInterface`。

## 回调接口列表

### 1. 应用命令回调

#### LtpAppCmdCallback
`com.letianpai.robot.letianpaiservice.LtpAppCmdCallback`

接收应用命令的回调接口。

**方法:**
- `onAppCommandReceived(String command, String data)` - 当接收到应用命令时调用

**参数:**
- `command` - 命令字符串
- `data` - 命令数据

**异常:**
- `RemoteException` - 远程调用异常

### 2. 音频效果回调

#### LtpAudioEffectCallback
`com.letianpai.robot.letianpaiservice.LtpAudioEffectCallback`

接收音频效果命令的回调接口。

**方法:**
- `onAudioEffectCommand(String command, String data)` - 当接收到音频效果命令时调用

### 3. 蓝牙回调

#### LtpBleCallback
`com.letianpai.robot.letianpaiservice.LtpBleCallback`

接收蓝牙命令的回调接口。

**方法:**
- `onBleCommandReceived(String command, String data)` - 当接收到蓝牙命令时调用

#### LtpBleResponseCallback
`com.letianpai.robot.letianpaiservice.LtpBleResponseCallback`

接收蓝牙响应的回调接口。

**方法:**
- `onBleResponseReceived(String command, String data)` - 当接收到蓝牙响应时调用

### 4. 表情回调

#### LtpExpressionCallback
`com.letianpai.robot.letianpaiservice.LtpExpressionCallback`

接收表情变化的回调接口。

**方法:**
- `onExpressionChanged(String command, String data)` - 当表情发生变化时调用

**参数:**
- `command` - 表情命令
- `data` - 表情数据

### 5. 识别命令回调

#### LtpIdentifyCmdCallback
`com.letianpai.robot.letianpaiservice.LtpIdentifyCmdCallback`

接收识别命令的回调接口。

**方法:**
- `onIdentifyCommandReceived(String command, String data)` - 当接收到识别命令时调用

### 6. 长连接回调

#### LtpLongConnectCallback
`com.letianpai.robot.letianpaiservice.LtpLongConnectCallback`

接收长连接命令的回调接口。

**方法:**
- `onLongConnectCommand(String command, String data)` - 当接收到长连接命令时调用

### 7. MCU命令回调

#### LtpMcuCommandCallback
`com.letianpai.robot.letianpaiservice.LtpMcuCommandCallback`

接收MCU命令的回调接口。

**方法:**
- `onMcuCommandReceived(String command, String data)` - 当接收到MCU命令时调用

### 8. 小米命令回调

#### LtpMiCmdCallback
`com.letianpai.robot.letianpaiservice.LtpMiCmdCallback`

接收小米命令的回调接口。

**方法:**
- `onMiCommandReceived(String command, String data)` - 当接收到小米命令时调用

### 9. 机器人状态回调

#### LtpRobotStatusCallback
`com.letianpai.robot.letianpaiservice.LtpRobotStatusCallback`

接收机器人状态变化的回调接口。

**方法:**
- `onRobotStatusChanged(String command, String data)` - 当机器人状态发生变化时调用

### 10. 传感器响应回调

#### LtpSensorResponseCallback
`com.letianpai.robot.letianpaiservice.LtpSensorResponseCallback`

接收传感器响应的回调接口。

**方法:**
- `onSensorResponseReceived(String command, String data)` - 当接收到传感器响应时调用

### 11. 语音回调

#### LtpSpeechCallback
`com.letianpai.robot.letianpaiservice.LtpSpeechCallback`

接收语音命令的回调接口。

**方法:**
- `onSpeechCommand(String command, String data)` - 当接收到语音命令时调用

### 12. TTS回调

#### LtpTTSCallback
`com.letianpai.robot.letianpaiservice.LtpTTSCallback`

接收TTS命令的回调接口。

**方法:**
- `onTTSCommand(String command, String data)` - 当接收到TTS命令时调用

**参数:**
- `command` - TTS命令
- `data` - TTS数据

### 13. 命令回调

#### LtpCommandCallback
`com.renhejia.robot.letianpaiservice.LtpCommandCallback`

通用命令回调接口。

**方法:**
- `onCommandReceived(String command, String data)` - 当接收到命令时调用

## 回调使用示例

### 1. 实现应用命令回调

```java
LtpAppCmdCallback appCmdCallback = new LtpAppCmdCallback.Stub() {
    @Override
    public void onAppCommandReceived(String command, String data) throws RemoteException {
        // 处理应用命令
        Log.d("RobotSDK", "App command received: " + command + ", data: " + data);
        
        // 根据命令类型进行不同处理
        switch (command) {
            case "start_app":
                // 启动应用
                break;
            case "stop_app":
                // 停止应用
                break;
            default:
                // 其他命令处理
                break;
        }
    }
};

// 注册回调
robotService.getAIDL().registerAppCmdCallback(appCmdCallback);
```

### 2. 实现TTS回调

```java
LtpTTSCallback ttsCallback = new LtpTTSCallback.Stub() {
    @Override
    public void onTTSCommand(String command, String data) throws RemoteException {
        // 处理TTS命令
        Log.d("RobotSDK", "TTS command: " + command + ", data: " + data);
        
        switch (command) {
            case "tts_start":
                // TTS开始播放
                break;
            case "tts_complete":
                // TTS播放完成
                break;
            case "tts_error":
                // TTS播放错误
                break;
        }
    }
};

// 注册TTS回调
robotService.getAIDL().registerTTSCallback(ttsCallback);
```

### 3. 实现表情回调

```java
LtpExpressionCallback expressionCallback = new LtpExpressionCallback.Stub() {
    @Override
    public void onExpressionChanged(String command, String data) throws RemoteException {
        // 处理表情变化
        Log.d("RobotSDK", "Expression changed: " + command + ", data: " + data);
        
        // 解析表情数据
        try {
            JSONObject expressionData = new JSONObject(data);
            String expressionType = expressionData.getString("type");
            String expressionStatus = expressionData.getString("status");
            
            // 根据表情状态更新UI
            updateExpressionUI(expressionType, expressionStatus);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }
};

// 注册表情回调
robotService.getAIDL().registerExpressionCallback(expressionCallback);
```

### 4. 实现传感器响应回调

```java
LtpSensorResponseCallback sensorCallback = new LtpSensorResponseCallback.Stub() {
    @Override
    public void onSensorResponseReceived(String command, String data) throws RemoteException {
        // 处理传感器响应
        Log.d("RobotSDK", "Sensor response: " + command + ", data: " + data);
        
        switch (command) {
            case "touch_sensor":
                // 触摸传感器响应
                handleTouchSensor(data);
                break;
            case "distance_sensor":
                // 距离传感器响应
                handleDistanceSensor(data);
                break;
            case "gyroscope":
                // 陀螺仪传感器响应
                handleGyroscope(data);
                break;
        }
    }
};

// 注册传感器响应回调
robotService.getAIDL().registerSensorResponseCallback(sensorCallback);
```

### 5. 实现MCU命令回调

```java
LtpMcuCommandCallback mcuCallback = new LtpMcuCommandCallback.Stub() {
    @Override
    public void onMcuCommandReceived(String command, String data) throws RemoteException {
        // 处理MCU命令
        Log.d("RobotSDK", "MCU command: " + command + ", data: " + data);
        
        switch (command) {
            case "motor_status":
                // 电机状态
                handleMotorStatus(data);
                break;
            case "battery_level":
                // 电池电量
                handleBatteryLevel(data);
                break;
            case "temperature":
                // 温度信息
                handleTemperature(data);
                break;
        }
    }
};

// 注册MCU命令回调
robotService.getAIDL().registerMcuCmdCallback(mcuCallback);
```

## 回调管理最佳实践

### 1. 回调注册时机

```java
public class RobotActivity extends Activity {
    private RobotService robotService;
    private LtpAppCmdCallback appCmdCallback;
    
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        // 初始化机器人服务
        robotService = RobotService.getInstance(this);
        
        // 创建回调实例
        createCallbacks();
    }
    
    @Override
    protected void onResume() {
        super.onResume();
        
        // 注册所有回调
        registerCallbacks();
    }
    
    @Override
    protected void onPause() {
        super.onPause();
        
        // 注销所有回调
        unregisterCallbacks();
    }
    
    private void createCallbacks() {
        appCmdCallback = new LtpAppCmdCallback.Stub() {
            @Override
            public void onAppCommandReceived(String command, String data) throws RemoteException {
                // 处理应用命令
            }
        };
    }
    
    private void registerCallbacks() {
        try {
            if (robotService.getAIDL() != null) {
                robotService.getAIDL().registerAppCmdCallback(appCmdCallback);
            }
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }
    
    private void unregisterCallbacks() {
        try {
            if (robotService.getAIDL() != null) {
                robotService.getAIDL().unregisterAppCmdCallback(appCmdCallback);
            }
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }
}
```

### 2. 线程安全处理

```java
LtpTTSCallback ttsCallback = new LtpTTSCallback.Stub() {
    @Override
    public void onTTSCommand(String command, String data) throws RemoteException {
        // 回调可能在非主线程中执行，需要切换到主线程更新UI
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                // 在主线程中更新UI
                updateTTSStatus(command, data);
            }
        });
        
        // 或者使用Handler
        handler.post(new Runnable() {
            @Override
            public void run() {
                updateTTSStatus(command, data);
            }
        });
    }
};
```

### 3. 错误处理

```java
private void registerCallback(LtpAppCmdCallback callback) {
    try {
        if (robotService.getAIDL() != null) {
            robotService.getAIDL().registerAppCmdCallback(callback);
        } else {
            Log.w("RobotSDK", "AIDL service is not available");
            // 延迟重试或提示用户
            retryRegisterCallback(callback);
        }
    } catch (RemoteException e) {
        Log.e("RobotSDK", "Failed to register callback", e);
        // 处理注册失败的情况
    }
}

private void retryRegisterCallback(LtpAppCmdCallback callback) {
    handler.postDelayed(new Runnable() {
        @Override
        public void run() {
            registerCallback(callback);
        }
    }, 1000); // 1秒后重试
}
```

## 注意事项

1. **内存泄漏**: 确保在适当的时机注销回调，避免内存泄漏
2. **线程安全**: 回调方法可能在非主线程中调用，注意线程安全
3. **异常处理**: 所有回调方法都可能抛出RemoteException，需要适当处理
4. **生命周期管理**: 在Activity/Service的生命周期方法中正确管理回调
5. **空指针检查**: 在使用AIDL服务前检查是否为null
6. **性能考虑**: 回调方法中避免执行耗时操作，必要时使用异步处理

---

*本文档基于反编译的class文件生成，具体的回调方法名称可能与实际实现略有不同，请以实际代码为准。*
