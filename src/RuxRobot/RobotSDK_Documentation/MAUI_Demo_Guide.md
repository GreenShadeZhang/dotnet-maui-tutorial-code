# Robot SDK MAUI 演示应用指南

## 概述

这个MAUI应用演示了如何在跨平台应用中集成和使用Robot SDK。应用提供了完整的机器人控制界面，包括动作控制、传感器监听、天线控制、表情和语音功能。

## 项目结构

```
MauiApp1/
├── Services/                           # 服务接口和实现
│   ├── IRobotControlService.cs         # 跨平台接口定义
│   └── DefaultRobotControlService.cs   # 默认实现（非Android平台）
├── Platforms/Android/Services/         # Android平台特定实现
│   └── AndroidRobotControlService.cs   # Android Robot SDK 实现
├── ViewModels/                         # MVVM视图模型
│   └── MainPageViewModel.cs            # 主页面视图模型
├── MainPage.xaml/.cs                   # 主界面
├── MauiProgram.cs                      # 依赖注入配置
└── ...

RobotSDK.Android.Binding/               # Android绑定库项目
├── RobotSDK.Android.Binding.csproj     # 绑定项目文件
└── Transforms/
    └── Metadata.xml                     # 绑定元数据配置
```

## 功能特性

### 🔌 连接管理
- **初始化服务**: 连接到Robot SDK服务
- **传感器控制**: 启动/停止传感器监听
- **电机控制**: 启用/禁用机器人电机

### 🎮 动作控制
- **基本移动**: 前进、后退、左转、右转
- **自定义动作**: 支持执行任意动作命令
- **动作参数**: 可配置速度、步数等参数

### 📡 天线控制
- **天线运动**: 挥手等预定义动作
- **灯光效果**: 红、绿、蓝灯光和关闭功能
- **自定义控制**: 支持自定义天线运动参数

### 😊 表情和语音
- **表情显示**: 开心、难过等表情
- **TTS语音**: 文字转语音播放
- **组合功能**: 同时播放语音和显示表情

### 📋 实时日志
- **操作记录**: 实时显示所有操作和事件
- **传感器事件**: 显示触摸、倾斜等传感器触发
- **错误信息**: 显示操作失败的详细信息

## 技术架构

### 跨平台设计
- **接口抽象**: `IRobotControlService` 定义统一接口
- **平台实现**: Android使用真实SDK，其他平台使用模拟实现
- **依赖注入**: 使用MAUI内置DI容器管理服务

### MVVM模式
- **ViewModel**: `MainPageViewModel` 处理业务逻辑
- **数据绑定**: XAML界面通过数据绑定与ViewModel交互
- **命令模式**: 使用ICommand处理用户操作

### Android绑定
- **AAR绑定**: 通过绑定库项目包装Android AAR
- **元数据配置**: 使用Metadata.xml优化C#绑定
- **条件引用**: 仅在Android平台引用绑定库

## 使用指南

### 1. 环境准备

#### 开发环境要求
- Visual Studio 2022 (17.8+) 或 Visual Studio Code
- .NET 9.0 SDK
- Android SDK (API 21+)
- Robot SDK AAR文件 (`RobotSdk-release-2.5.aar`)

#### AAR文件配置
1. 将 `RobotSdk-release-2.5.aar` 放置在项目根目录
2. 确保绑定项目正确引用AAR文件
3. 检查Metadata.xml配置是否正确

### 2. 编译和运行

#### 编译项目
```bash
# 清理解决方案
dotnet clean

# 恢复NuGet包
dotnet restore

# 编译Android版本
dotnet build -f net9.0-android
```

#### 运行应用
```bash
# 在Android设备/模拟器上运行
dotnet run -f net9.0-android
```

### 3. 应用操作流程

#### 初始化连接
1. 启动应用后，点击"初始化机器人服务"
2. 观察状态显示是否为"已连接"
3. 查看日志确认初始化成功

#### 启用功能
1. 点击"启动传感器"开始监听传感器事件
2. 点击"启用电机"允许执行动作命令
3. 观察日志中的状态变化

#### 测试功能
1. **动作测试**: 使用方向按钮测试机器人移动
2. **天线测试**: 测试天线挥手和灯光效果
3. **语音测试**: 点击问候按钮测试语音播放
4. **舞蹈演示**: 点击"执行舞蹈"查看组合动作

### 4. 传感器事件

应用会自动监听以下传感器事件：
- **🖱️ 单击/双击/长按**: 触摸传感器事件
- **⬅️➡️⬆️⬇️ 倾斜**: 机器人倾斜方向检测
- **📡 TOF传感器**: 距离检测传感器

## 代码示例

### 自定义动作
```csharp
// 执行自定义动作
await robotService.PerformActionAsync(
    actionNumber: 63,  // 前进动作
    speed: 50,         // 速度
    steps: 1           // 步数
);
```

### 天线控制
```csharp
// 控制天线运动
await robotService.MoveAntennaAsync(
    cmd: 1,      // 命令类型
    step: 10,    // 步数
    speed: 50,   // 速度
    angle: 45    // 角度
);

// 设置天线灯光
await robotService.SetAntennaLightAsync(RobotColors.Red);
```

### 语音和表情
```csharp
// 播放语音并显示表情
await robotService.SpeakWithExpressionAsync(
    text: "Hello, World!",
    expression: RobotExpressions.Happy
);
```

### 传感器事件处理
```csharp
// 订阅传感器事件
robotService.TapDetected += (s, e) => {
    // 处理单击事件
    Debug.WriteLine("机器人被单击了！");
};
```

## 故障排除

### 常见问题

#### 1. 服务初始化失败
**症状**: 初始化后状态显示"连接失败"
**解决方案**:
- 确认在Android设备上运行
- 检查Robot SDK服务是否正在运行
- 查看日志中的详细错误信息

#### 2. 动作命令无响应
**症状**: 点击动作按钮无反应
**解决方案**:
- 确认电机已启用
- 检查机器人硬件连接
- 验证命令参数是否正确

#### 3. 传感器事件不触发
**症状**: 传感器监听已启动但无事件
**解决方案**:
- 确认传感器监听已正确启动
- 检查机器人传感器硬件
- 在模拟实现中会有3秒延迟的模拟事件

#### 4. 绑定库编译错误
**症状**: 编译时出现绑定相关错误
**解决方案**:
- 检查AAR文件路径
- 验证Metadata.xml配置
- 清理并重新编译项目

### 调试技巧

#### 启用详细日志
```csharp
// 在MauiProgram.cs中启用详细日志
#if DEBUG
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
#endif
```

#### 查看应用日志
- 使用Visual Studio的输出窗口查看调试信息
- 在应用界面的日志区域查看操作记录
- 使用Android Studio的Logcat查看详细日志

## 扩展开发

### 添加新功能
1. 在 `IRobotControlService` 中定义新接口
2. 在Android实现中添加具体功能
3. 在ViewModel中添加对应命令
4. 在XAML中添加UI元素

### 自定义机器人行为
```csharp
public async Task CustomBehaviorAsync()
{
    // 组合多个动作创建自定义行为
    await robotService.SpeakAsync("开始自定义行为");
    await robotService.ShowExpressionAsync(RobotExpressions.Excited);
    await robotService.SetAntennaLightAsync(RobotColors.Purple);
    await robotService.MoveForwardAsync(30, 2);
    await robotService.TurnRightAsync(40, 1);
    await robotService.MoveAntennaAsync(2, 20, 30, 90);
    await robotService.SpeakAsync("自定义行为完成");
}
```

### 添加新的传感器支持
1. 在接口中添加新事件
2. 在Android实现中订阅相应回调
3. 在ViewModel中处理新事件
4. 更新UI显示新传感器状态

## 性能优化

### 内存管理
- 及时清理事件订阅
- 在页面销毁时调用`DisposeAsync()`
- 避免在回调中创建大量对象

### 响应性能
- 使用异步方法避免UI阻塞
- 在后台线程处理耗时操作
- 合理使用Task.Delay控制动作间隔

### 网络和通信
- 实现断线重连机制
- 添加操作超时处理
- 缓存常用的配置参数

## 总结

这个MAUI演示应用展示了如何：
- 在跨平台应用中集成Android原生SDK
- 使用MVVM模式构建清晰的架构
- 实现丰富的机器人控制功能
- 提供友好的用户界面和实时反馈

通过这个演示，开发者可以学习到Robot SDK的完整使用方法，并将其应用到实际的机器人应用开发中。

---

**开发者**: MAUI Robot SDK Team  
**版本**: 1.0  
**更新日期**: 2025年7月10日
