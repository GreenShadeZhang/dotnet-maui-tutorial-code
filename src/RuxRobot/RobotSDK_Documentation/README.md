# 🤖 Robot SDK MAUI Demo

一个展示如何在.NET MAUI应用中集成和使用Robot SDK的完整演示项目。

## 📋 项目概述

本项目包含：
- 🔧 **Robot SDK AAR绑定库** - Android原生库的C#绑定
- 📱 **MAUI跨平台应用** - 完整的机器人控制界面
- 📚 **完整文档** - 从反编译到集成的全套指南
- 🎯 **演示功能** - 涵盖所有主要Robot SDK功能

## 🏗️ 项目结构

```
📦 MauiApp1/
├── 📁 RobotSDK_Documentation/          # 完整文档集合
│   ├── 📄 README.md                    # 文档索引
│   ├── 📄 RobotSDK_API_Documentation.md
│   ├── 📄 RobotSDK_Callbacks_Documentation.md
│   ├── 📄 RobotSDK_QuickStart_Guide.md
│   └── 📄 MAUI_Demo_Guide.md
├── 📁 RobotSDK.Android.Binding/        # Android绑定库
│   ├── 📄 RobotSDK.Android.Binding.csproj
│   └── 📁 Transforms/
├── 📁 MauiApp1/                        # 主应用项目
│   ├── 📁 Services/                    # 服务层
│   ├── 📁 ViewModels/                  # MVVM视图模型
│   ├── 📁 Platforms/Android/Services/  # Android平台实现
│   └── 📄 MainPage.xaml                # 主界面
├── 📄 RobotSdk-release-2.5.aar         # Robot SDK库文件
└── 📄 MauiApp1.sln                     # 解决方案文件
```

## ✨ 功能特性

### 🎮 机器人控制
- ⬆️⬇️⬅️➡️ **方向控制** - 前进、后退、左转、右转
- ⚡ **电机管理** - 启用/禁用电机控制
- 🎯 **自定义动作** - 支持任意动作命令

### 📡 传感器监听
- 🖱️ **触摸检测** - 单击、双击、长按
- 📐 **倾斜感应** - 前后左右倾斜检测
- 📡 **TOF传感器** - 距离检测

### 🌈 天线控制
- 🌊 **运动控制** - 天线挥手等动作
- 🔴🟢🔵 **灯光效果** - 多彩灯光控制
- ⚫ **灯光开关** - 一键关闭所有灯光

### 😊 表情语音
- 😊😢 **表情显示** - 丰富的机器人表情
- 🗣️ **TTS语音** - 文字转语音播放
- 🎭 **组合表演** - 语音+表情同步播放

### 💃 智能演示
- 🎪 **舞蹈序列** - 完整的动作编排
- 📋 **实时日志** - 所有操作的详细记录
- 🔄 **状态监控** - 实时显示连接和运行状态

## 🚀 快速开始

### 1️⃣ 环境准备

```bash
# 必需工具
✅ Visual Studio 2022 (17.8+)
✅ .NET 9.0 SDK
✅ Android SDK (API 21+)
✅ Robot SDK AAR文件
```

### 2️⃣ 项目配置

```bash
# 克隆或下载项目
git clone <project-url>
cd MauiApp1

# 恢复依赖
dotnet restore

# 编译项目
dotnet build
```

### 3️⃣ 运行演示

```bash
# 在Android设备上运行
dotnet run -f net9.0-android

# 或在Visual Studio中直接运行
```

### 4️⃣ 使用应用

1. 📱 启动应用后点击 **"初始化机器人服务"**
2. 🎯 点击 **"启动传感器"** 开始监听
3. ⚡ 点击 **"启用电机"** 允许动作控制
4. 🎮 使用各种按钮测试机器人功能
5. 📋 观察日志区域的实时反馈

## 📚 文档指南

### 🔰 新手入门
👉 从 [`RobotSDK_QuickStart_Guide.md`](RobotSDK_Documentation/RobotSDK_QuickStart_Guide.md) 开始

### 📖 完整API
👉 查看 [`RobotSDK_API_Documentation.md`](RobotSDK_Documentation/RobotSDK_API_Documentation.md)

### 📞 回调处理
👉 参考 [`RobotSDK_Callbacks_Documentation.md`](RobotSDK_Documentation/RobotSDK_Callbacks_Documentation.md)

### 🎯 MAUI集成
👉 阅读 [`MAUI_Demo_Guide.md`](RobotSDK_Documentation/MAUI_Demo_Guide.md)

## 🎯 演示场景

### 🏠 智能家居助手
```csharp
// 温度播报示例
await robotService.SpeakWithExpressionAsync(
    "当前温度是25度，很舒适", 
    RobotExpressions.Happy
);
await robotService.SetAntennaLightAsync(RobotColors.Green);
```

### 🎮 互动游戏
```csharp
// 游戏响应示例
robotService.TapDetected += async (s, e) => {
    await robotService.SpeakAsync("游戏开始！");
    await robotService.PerformActionAsync(RobotActionCommands.MoveForward);
};
```

### 🎭 娱乐表演
```csharp
// 舞蹈表演示例
await PerformDanceRoutineAsync(); // 完整舞蹈序列
```

## 🔧 技术架构

### 📱 跨平台支持
- **Android**: 真实Robot SDK集成
- **其他平台**: 模拟实现（用于开发测试）

### 🏗️ 架构模式
- **MVVM**: Model-View-ViewModel模式
- **DI**: 依赖注入管理服务
- **Async/Await**: 异步编程模式

### 🔗 绑定技术
- **AAR绑定**: Android原生库绑定
- **元数据配置**: 优化C#绑定体验
- **条件编译**: 平台特定代码隔离

## 🛠️ 开发指南

### 添加新功能
1. 在 `IRobotControlService` 定义接口
2. 在Android实现中添加具体逻辑
3. 在ViewModel中添加命令绑定
4. 在XAML中添加UI元素

### 自定义机器人行为
```csharp
public async Task CustomBehaviorAsync()
{
    await robotService.SpeakAsync("开始自定义动作");
    await robotService.ShowExpressionAsync(RobotExpressions.Thinking);
    // ... 自定义动作序列
}
```

### 传感器事件处理
```csharp
robotService.TapDetected += async (s, e) => {
    // 自定义响应逻辑
    await HandleTapEvent();
};
```

## 🐛 故障排除

### ❌ 常见问题

| 问题 | 原因 | 解决方案 |
|------|------|----------|
| 服务初始化失败 | Robot SDK服务未运行 | 检查Android设备环境 |
| 动作无响应 | 电机未启用 | 先点击"启用电机" |
| 传感器无事件 | 传感器未启动 | 点击"启动传感器" |
| 编译错误 | AAR路径问题 | 检查绑定项目配置 |

### 🔍 调试技巧
- 📋 查看应用内的实时日志
- 🖥️ 使用Visual Studio输出窗口
- 📱 使用Android Logcat查看详细信息

## 📊 性能特性

- ⚡ **响应迅速**: 异步操作避免UI阻塞
- 💾 **内存优化**: 正确的资源管理和事件清理
- 🔄 **错误恢复**: 完善的异常处理和重试机制
- 📱 **跨平台**: 统一API在不同平台上的一致体验

## 🤝 贡献指南

欢迎提交Issue和Pull Request：
1. 🍴 Fork本项目
2. 🌿 创建功能分支
3. 💻 开发新功能
4. ✅ 提交Pull Request

## 📄 开源许可

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。

## 🙏 致谢

- Robot SDK 开发团队
- .NET MAUI 社区
- 所有贡献者和测试者

---

**🎯 立即开始你的机器人应用开发之旅！**

如有疑问，请查看 [文档目录](RobotSDK_Documentation/README.md) 或提交Issue。
