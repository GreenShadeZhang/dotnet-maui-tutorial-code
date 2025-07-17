# Robot SDK 快速开始指南

## 前提条件

1. Android开发环境 (Android Studio)
2. 最小API级别: 根据机器人系统要求
3. Robot SDK AAR文件: `RobotSdk-release-2.5.aar`

## 集成步骤

### 1. 添加AAR依赖

将`RobotSdk-release-2.5.aar`文件添加到您的Android项目中：

#### 方法一: 通过Android Studio
1. 在Android Studio中，选择 `File > New > New Module`
2. 选择 `Import .JAR/.AAR Package`
3. 选择您的AAR文件
4. 在app模块的`build.gradle`中添加依赖：

```gradle
dependencies {
    implementation project(':robotsdk-release-2.5')
}
```

#### 方法二: 直接放置AAR文件
1. 在app模块下创建`libs`目录
2. 将AAR文件复制到`libs`目录
3. 在app模块的`build.gradle`中添加：

```gradle
android {
    repositories {
        flatDir {
            dirs 'libs'
        }
    }
}

dependencies {
    implementation(name: 'RobotSdk-release-2.5', ext: 'aar')
}
```

### 2. 添加权限

在`AndroidManifest.xml`中添加必要的权限：

```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<uses-permission android:name="android.permission.RECORD_AUDIO" />
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
<!-- 根据需要添加其他权限 -->
```

### 3. 初始化SDK

在您的Activity或Application中初始化Robot SDK：

```java
public class MainActivity extends AppCompatActivity {
    private RobotService robotService;
    
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        
        // 初始化Robot SDK
        initRobotSDK();
    }
    
    private void initRobotSDK() {
        // 获取RobotService实例
        robotService = RobotService.getInstance(this);
    }
    
    @Override
    protected void onDestroy() {
        super.onDestroy();
        
        // 清理资源
        if (robotService != null) {
            robotService.unbindService();
        }
    }
}
```

## 基本功能示例

### 1. 传感器事件处理

```java
public class RobotSensorManager {
    private RobotService robotService;
    private SensorCallback sensorCallback;
    
    public RobotSensorManager(Context context) {
        robotService = RobotService.getInstance(context);
        setupSensorCallback();
    }
    
    private void setupSensorCallback() {
        sensorCallback = new SensorCallback() {
            @Override
            public void onTapResponse() {
                Log.d("Robot", "Robot was tapped");
                // 处理单击事件
                handleTapEvent();
            }
            
            @Override
            public void onDoubleTapResponse() {
                Log.d("Robot", "Robot was double tapped");
                // 处理双击事件
                handleDoubleTapEvent();
            }
            
            @Override
            public void onLongPressResponse() {
                Log.d("Robot", "Robot was long pressed");
                // 处理长按事件
                handleLongPressEvent();
            }
            
            @Override
            public void onFallBackend() {
                Log.d("Robot", "Robot fell backward");
                // 处理后倾事件
                handleFallEvent("backward");
            }
            
            @Override
            public void onFallForward() {
                Log.d("Robot", "Robot fell forward");
                // 处理前倾事件
                handleFallEvent("forward");
            }
            
            @Override
            public void onFallRight() {
                Log.d("Robot", "Robot fell right");
                // 处理右倾事件
                handleFallEvent("right");
            }
            
            @Override
            public void onFallLeft() {
                Log.d("Robot", "Robot fell left");
                // 处理左倾事件
                handleFallEvent("left");
            }
            
            @Override
            public void onTof() {
                Log.d("Robot", "TOF sensor triggered");
                // 处理TOF传感器事件
                handleTofEvent();
            }
        };
    }
    
    public void startSensorMonitoring() {
        try {
            robotService.robotRegisterSensorCallback(sensorCallback);
            robotService.robotOpenSensor();
        } catch (RemoteException e) {
            Log.e("Robot", "Failed to start sensor monitoring", e);
        }
    }
    
    public void stopSensorMonitoring() {
        try {
            robotService.robotUnregisterSensor();
            robotService.robotCloseSensor();
        } catch (RemoteException e) {
            Log.e("Robot", "Failed to stop sensor monitoring", e);
        }
    }
    
    private void handleTapEvent() {
        // 单击响应 - 例如播放问候语
        robotService.robotPlayTTs("Hello! You tapped me!");
    }
    
    private void handleDoubleTapEvent() {
        // 双击响应 - 例如执行特定动作
        ActionMessage action = new ActionMessage();
        action.setNumber(63); // 前进
        action.setSpeed(50);
        action.setStepNum(1);
        robotService.robotActionCommand(action);
    }
    
    private void handleLongPressEvent() {
        // 长按响应 - 例如改变表情
        robotService.robotStartExpression("happy");
    }
    
    private void handleFallEvent(String direction) {
        // 倾斜响应 - 例如播放警告
        robotService.robotPlayTTs("I'm falling " + direction + "!");
    }
    
    private void handleTofEvent() {
        // TOF传感器响应 - 例如避障
        robotService.robotPlayTTs("Obstacle detected!");
    }
}
```

### 2. 机器人动作控制

```java
public class RobotActionController {
    private RobotService robotService;
    
    public RobotActionController(Context context) {
        robotService = RobotService.getInstance(context);
    }
    
    // 前进
    public void moveForward() {
        ActionMessage action = new ActionMessage();
        action.setNumber(Integer.parseInt(RobotRemoteConsts.MOVE_FORWARD)); // 63
        action.setSpeed(50);
        action.setStepNum(1);
        robotService.robotActionCommand(action);
    }
    
    // 后退
    public void moveBackward() {
        ActionMessage action = new ActionMessage();
        action.setNumber(Integer.parseInt(RobotRemoteConsts.WALK_BACKWARD)); // 64
        action.setSpeed(50);
        action.setStepNum(1);
        robotService.robotActionCommand(action);
    }
    
    // 左转
    public void turnLeft() {
        ActionMessage action = new ActionMessage();
        action.setNumber(Integer.parseInt(RobotRemoteConsts.TURN_LEFT)); // 5
        action.setSpeed(50);
        action.setStepNum(1);
        robotService.robotActionCommand(action);
    }
    
    // 右转
    public void turnRight() {
        ActionMessage action = new ActionMessage();
        action.setNumber(Integer.parseInt(RobotRemoteConsts.GO_RIGHT)); // 6
        action.setSpeed(50);
        action.setStepNum(1);
        robotService.robotActionCommand(action);
    }
    
    // 自定义动作
    public void performCustomAction(int actionNumber, int speed, int steps) {
        ActionMessage action = new ActionMessage();
        action.setNumber(actionNumber);
        action.setSpeed(speed);
        action.setStepNum(steps);
        robotService.robotActionCommand(action);
    }
    
    // 开启电机
    public void enableMotor() {
        robotService.robotOpenMotor();
    }
    
    // 关闭电机
    public void disableMotor() {
        robotService.robotCloseMotor();
    }
}
```

### 3. 天线控制

```java
public class RobotAntennaController {
    private RobotService robotService;
    
    public RobotAntennaController(Context context) {
        robotService = RobotService.getInstance(context);
    }
    
    // 控制天线运动
    public void moveAntenna(int cmd, int step, int speed, int angle) {
        AntennaMessage antennaMessage = new AntennaMessage();
        antennaMessage.set(cmd, step, speed, angle);
        robotService.robotAntennaMotion(antennaMessage);
    }
    
    // 设置天线灯光
    public void setAntennaLight(int color) {
        AntennaLightMessage lightMessage = new AntennaLightMessage();
        lightMessage.set(color);
        robotService.robotAntennaLight(lightMessage);
    }
    
    // 关闭天线灯光
    public void turnOffAntennaLight() {
        robotService.robotCloseAntennaLight();
    }
    
    // 预设天线动作
    public void antennaWave() {
        // 天线挥手动作
        moveAntenna(1, 10, 50, 45);
    }
    
    public void antennaRotate() {
        // 天线旋转动作
        moveAntenna(2, 20, 30, 180);
    }
    
    // 预设灯光效果
    public void setRedLight() {
        setAntennaLight(0xFF0000); // 红色
    }
    
    public void setGreenLight() {
        setAntennaLight(0x00FF00); // 绿色
    }
    
    public void setBlueLight() {
        setAntennaLight(0x0000FF); // 蓝色
    }
}
```

### 4. 语音和表情控制

```java
public class RobotExpressionController {
    private RobotService robotService;
    
    public RobotExpressionController(Context context) {
        robotService = RobotService.getInstance(context);
    }
    
    // 播放TTS语音
    public void speak(String text) {
        robotService.robotPlayTTs(text);
    }
    
    // 播放预设语音
    public void playGreeting() {
        speak("Hello! I'm your robot assistant!");
    }
    
    public void playGoodbye() {
        speak("Goodbye! See you later!");
    }
    
    // 表情控制
    public void showHappyExpression() {
        robotService.robotStartExpression("happy");
    }
    
    public void showSadExpression() {
        robotService.robotStartExpression("sad");
    }
    
    public void showSurprisedExpression() {
        robotService.robotStartExpression("surprised");
    }
    
    public void showAngryExpression() {
        robotService.robotStartExpression("angry");
    }
    
    public void changeExpression(String expression) {
        robotService.robotChangeExpression(expression);
    }
    
    public void stopExpression() {
        robotService.robotStopExpression();
    }
    
    // 组合动作 - 说话并显示表情
    public void speakWithExpression(String text, String expression) {
        showHappyExpression();
        speak(text);
        
        // 延迟停止表情（根据语音长度调整）
        new Handler().postDelayed(() -> {
            stopExpression();
        }, text.length() * 100); // 简单的时间估算
    }
}
```

### 5. 完整的使用示例

```java
public class RobotDemoActivity extends AppCompatActivity {
    private RobotSensorManager sensorManager;
    private RobotActionController actionController;
    private RobotAntennaController antennaController;
    private RobotExpressionController expressionController;
    
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_robot_demo);
        
        // 初始化控制器
        initControllers();
        
        // 设置UI事件
        setupUI();
        
        // 开始传感器监听
        sensorManager.startSensorMonitoring();
        
        // 初始化问候
        expressionController.speakWithExpression("Hello! I'm ready to help you!", "happy");
    }
    
    private void initControllers() {
        sensorManager = new RobotSensorManager(this);
        actionController = new RobotActionController(this);
        antennaController = new RobotAntennaController(this);
        expressionController = new RobotExpressionController(this);
    }
    
    private void setupUI() {
        findViewById(R.id.btn_forward).setOnClickListener(v -> {
            actionController.moveForward();
            expressionController.speak("Moving forward");
        });
        
        findViewById(R.id.btn_backward).setOnClickListener(v -> {
            actionController.moveBackward();
            expressionController.speak("Moving backward");
        });
        
        findViewById(R.id.btn_left).setOnClickListener(v -> {
            actionController.turnLeft();
            expressionController.speak("Turning left");
        });
        
        findViewById(R.id.btn_right).setOnClickListener(v -> {
            actionController.turnRight();
            expressionController.speak("Turning right");
        });
        
        findViewById(R.id.btn_happy).setOnClickListener(v -> {
            expressionController.speakWithExpression("I'm so happy!", "happy");
            antennaController.setGreenLight();
        });
        
        findViewById(R.id.btn_sad).setOnClickListener(v -> {
            expressionController.speakWithExpression("I'm feeling sad", "sad");
            antennaController.setBlueLight();
        });
        
        findViewById(R.id.btn_wave).setOnClickListener(v -> {
            antennaController.antennaWave();
            expressionController.speak("Hello there!");
        });
        
        findViewById(R.id.btn_dance).setOnClickListener(v -> {
            performDanceRoutine();
        });
    }
    
    private void performDanceRoutine() {
        expressionController.speak("Let me dance for you!");
        expressionController.showHappyExpression();
        
        // 简单的舞蹈序列
        new Handler().postDelayed(() -> {
            actionController.turnLeft();
            antennaController.setRedLight();
        }, 1000);
        
        new Handler().postDelayed(() -> {
            actionController.turnRight();
            antennaController.setGreenLight();
        }, 2000);
        
        new Handler().postDelayed(() -> {
            antennaController.antennaWave();
            antennaController.setBlueLight();
        }, 3000);
        
        new Handler().postDelayed(() -> {
            expressionController.speak("That was fun!");
            antennaController.turnOffAntennaLight();
            expressionController.stopExpression();
        }, 4000);
    }
    
    @Override
    protected void onDestroy() {
        super.onDestroy();
        
        // 清理资源
        if (sensorManager != null) {
            sensorManager.stopSensorMonitoring();
        }
        
        if (antennaController != null) {
            antennaController.turnOffAntennaLight();
        }
        
        if (expressionController != null) {
            expressionController.stopExpression();
        }
    }
}
```

## 常见问题解决

### 1. 服务连接失败
```java
// 检查服务是否可用
if (robotService.getAIDL() == null) {
    Log.w("Robot", "Robot service not available, retrying...");
    // 延迟重试
    new Handler().postDelayed(() -> {
        // 重新初始化
        robotService = RobotService.getInstance(this);
    }, 1000);
}
```

### 2. 权限请求（Android 6.0+）
```java
private void requestPermissions() {
    String[] permissions = {
        Manifest.permission.RECORD_AUDIO,
        Manifest.permission.WRITE_EXTERNAL_STORAGE
    };
    
    ActivityCompat.requestPermissions(this, permissions, REQUEST_PERMISSIONS);
}
```

### 3. 异常处理
```java
try {
    robotService.robotActionCommand(actionMessage);
} catch (Exception e) {
    Log.e("Robot", "Command failed", e);
    // 显示错误信息给用户
    Toast.makeText(this, "Robot command failed: " + e.getMessage(), Toast.LENGTH_SHORT).show();
}
```

## 下一步

1. 查看完整的API文档了解更多功能
2. 实现自定义回调处理更复杂的交互
3. 集成语音识别和自然语言处理
4. 开发自定义的机器人行为模式
5. 优化性能和用户体验

---

*这个快速开始指南提供了Robot SDK的基本使用方法。更多高级功能请参考详细的API文档。*
