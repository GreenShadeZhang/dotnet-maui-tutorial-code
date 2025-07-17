# MP4表情播放器

这是一个基于.NET MAUI的表情播放器应用程序，主要针对Android平台。

## 功能特性

- 支持播放MP4格式的表情视频
- 表情按情绪分类（开心、愤怒、伤心、惊讶、睡觉等）
- 快速情绪选择按钮
- 随机表情播放
- 表情列表浏览和选择

## 已解决的技术问题

### 1. LocalBroadcastManager 兼容性问题
**问题**: `AndroidX.LocalBroadcastManager.Content.LocalBroadcastManager not supported on Android 13 and above`

**解决方案**:
- 降级到兼容版本：`CommunityToolkit.Maui v9.0.3` 和 `CommunityToolkit.Maui.MediaElement v4.0.1`
- 创建了简化的Android视频服务，避免LocalBroadcastManager依赖
- 使用平台特定的依赖注入

### 2. 文件资源管理优化
**改进**:
- 表情映射文件：`Resources/Raw/emotions.json`
- 视频文件：`Resources/Raw/videos/`
- 使用MAUI的Raw资源系统
- 跨平台文件访问API：`FileSystem.OpenAppPackageFileAsync()`

### 4. Android MediaElement路径问题修复
**问题**: `ExoPlaybackException: unknown protocol: ms-appx`

**解决方案**:
- 使用正确的Android资源路径格式：`MediaSource.FromResource("videos/{fileName}")`
- Android服务改用 `embed://videos/{fileName}` 作为回退URI
- 实现多级回退机制：首选 `MediaSource.FromResource()`，回退到平台特定URI，最后回退到临时文件

### 3. 平台特定视频URI处理
**Android**: 使用简化的相对路径 `videos/{fileName}`
**Windows**: 使用 `ms-appx:///Resources/Raw/videos/{fileName}`

## 项目结构

```
Mp4EmojisPlayer/
├── Models/
│   └── EmotionModel.cs          # 表情数据模型
├── Services/
│   ├── IVideoService.cs         # 视频服务接口
│   └── EmotionService.cs        # 表情业务逻辑
├── Platforms/
│   ├── Android/Services/
│   │   └── SimpleVideoService.cs   # Android视频服务
│   └── Windows/Services/
│       └── VideoService.cs         # Windows视频服务
├── Resources/Raw/
│   ├── emotions.json            # 表情映射文件
│   └── videos/                  # MP4表情视频文件
└── MainPage.xaml/.cs            # 主界面
```

## 构建和运行

### Android
```bash
dotnet clean Mp4EmojisPlayer\Mp4EmojisPlayer.csproj
dotnet build Mp4EmojisPlayer\Mp4EmojisPlayer.csproj -f net9.0-android
dotnet run Mp4EmojisPlayer\Mp4EmojisPlayer.csproj -f net9.0-android
```

### Windows
```bash
dotnet build Mp4EmojisPlayer\Mp4EmojisPlayer.csproj -f net9.0-windows10.0.19041.0
```

## 部署注意事项

### Android设备调试
1. **垃圾回收警告**: 日志中的GC消息是正常的，表示内存管理工作正常
2. **LocalBroadcastManager警告**: 已通过版本降级解决，不影响功能
3. **视频播放**: 支持循环播放和媒体控件

### 性能优化
- 使用简化的视频服务减少依赖
- 异步文件访问
- 内存友好的视频URI生成

## 表情映射格式

```json
[
    {
        "cmd_tag": "h0001",
        "cmd_tag_name": "愤怒"
    },
    {
        "cmd_tag": "h0002", 
        "cmd_tag_name": "傲慢"
    }
]
```

- `cmd_tag`: 表情编号（对应视频文件名）
- `cmd_tag_name`: 表情名称
- 部分没有标注名称，应该是预留暂未使用的

## 技术栈

- **.NET 9 MAUI** - 跨平台框架
- **CommunityToolkit.Maui v9.0.3** - MAUI社区工具包
- **CommunityToolkit.Maui.MediaElement v4.0.1** - 视频播放组件
- **跨平台文件系统访问** - FileSystem API
- **依赖注入** - 平台特定服务

## 已测试环境

- ✅ Android 13+ 设备
- ✅ Windows 10/11
- ✅ 视频文件格式：MP4
- ✅ 表情数量：80+ 个表情视频