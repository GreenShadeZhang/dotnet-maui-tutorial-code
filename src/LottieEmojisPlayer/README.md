# Lottie 动画预览器 (MAUI版)

这是一个基于 .NET MAUI 和 SkiaSharp 构建的 Lottie 动画预览应用，参考了 [LottieSharp](https://github.com/quicoli/LottieSharp) 项目的实现。

## 功能特性

- ✨ 预览 Lottie JSON 动画文件
- 🎮 播放/停止控制
- 🔄 支持循环播放
- 📱 支持 Android 和 Windows 平台
- 📊 显示动画详细信息（版本、时长、帧率等）
- 🎨 现代化的 Material Design UI

## 技术栈

- **.NET 9.0** - 最新的 .NET 框架
- **MAUI** - 跨平台 UI 框架
- **SkiaSharp** - 2D 图形渲染引擎
- **SkiaSharp.Skottie** - Lottie 动画渲染引擎
- **MVVM模式** - 数据绑定和命令模式

## 项目结构

```
LottieEmojisPlayer/
├── Controls/
│   └── LottieAnimationView.cs          # 自定义 Lottie 动画控件
├── ViewModels/
│   └── MainPageViewModel.cs            # 主页面视图模型
├── Models/
│   ├── AnimationInfo.cs                # 动画信息模型
│   ├── LottieFileItem.cs               # Lottie 文件项模型
│   ├── RepeatMode.cs                    # 重复模式枚举
│   └── LottieDefaults.cs               # 默认常量
├── Converters/
│   ├── BoolToColorConverter.cs         # 布尔值到颜色转换器
│   └── StringToBoolConverter.cs        # 字符串到布尔值转换器
├── Resources/Raw/lottiefiles/           # Lottie 动画文件目录
│   ├── ask.json
│   ├── h0001.mp4.lottie.json
│   ├── h0065.mp4.lottie.json
│   ├── look.json
│   ├── sad.json
│   ├── speak.json
│   └── think.json
└── MainPage.xaml                       # 主页面 UI
```

## 核心实现

### LottieAnimationView 控件

基于 `SKCanvasView` 自定义的 Lottie 动画播放控件，主要特性：

- 支持从 MAUI Raw 资源加载动画文件
- 实现动画播放、停止、循环控制
- 提供动画信息绑定（版本、时长、帧率等）
- 自动处理 MAUI 文件系统路径

### 关键属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `FilePath` | `string` | Lottie 文件路径 |
| `AutoPlay` | `bool` | 是否自动播放 |
| `RepeatCount` | `int` | 重复次数 (-1 为无限循环) |
| `IsPlaying` | `bool` | 播放状态 |
| `Repeat` | `RepeatMode` | 重复模式 (重新开始/反向) |
| `Info` | `AnimationInfo` | 动画详细信息 |

### 关键方法

| 方法 | 说明 |
|------|------|
| `PlayAnimation()` | 开始播放动画 |
| `StopAnimation()` | 停止播放动画 |

## MAUI 特殊处理

### 文件路径处理

由于 MAUI 的资源系统与传统 .NET 应用不同，项目做了以下适配：

1. **Raw 资源访问**：使用 `FileSystem.OpenAppPackageFileAsync()` 访问 Raw 资源
2. **流处理**：将文件流复制到内存流，确保 SkiaSharp 能正确读取
3. **路径解析**：自动判断相对路径和绝对路径

### Android 特殊注意事项

- Raw 资源文件在 Android 平台会被打包到 APK 中
- 文件路径使用正斜杠 `/` 分隔符
- 支持嵌套目录结构

## 构建和运行

### 前提条件

- Visual Studio 2022 (17.8 或更高版本)
- .NET 9.0 SDK
- MAUI 工作负载

### 构建命令

```bash
# 还原 NuGet 包
dotnet restore

# 构建项目
dotnet build

# 运行 Android 版本
dotnet build -t:Run -f net9.0-android

# 运行 Windows 版本
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

## NuGet 依赖

```xml
<PackageReference Include="SkiaSharp.Resources" Version="3.119.0" />
<PackageReference Include="SkiaSharp.Skottie" Version="3.119.0" />
<PackageReference Include="SkiaSharp.Views.Maui.Controls" Version="3.119.0" />
```

## 使用方法

1. 将 Lottie JSON 文件放入 `Resources/Raw/lottiefiles/` 目录
2. 启动应用
3. 从文件列表中选择要预览的动画
4. 使用播放/停止按钮控制动画
5. 查看动画详细信息

## 参考项目

本项目的 Lottie 动画渲染核心逻辑参考了 [LottieSharp](https://github.com/quicoli/LottieSharp) 项目，并根据 MAUI 环境进行了适配和优化。

## 许可证

本项目基于 MIT 许可证开源。
