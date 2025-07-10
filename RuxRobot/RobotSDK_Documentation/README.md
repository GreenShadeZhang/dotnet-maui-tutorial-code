# Robot SDK 完整文档索引

## 概述

本文档集合基于Robot SDK 2.5版本的AAR包反编译分析生成，提供了完整的API接口文档和使用指南。

## 文档结构

### 📋 1. API接口文档
**文件**: `RobotSDK_API_Documentation.md`

**内容包括**:
- SDK核心架构概述
- 主要类和接口详细说明
- RobotService核心服务类
- ILetianpaiService AIDL接口
- 消息类（ActionMessage、AntennaMessage等）
- 常量定义和使用示例

**适用于**: 需要了解完整API的开发者

### 📞 2. 回调接口文档
**文件**: `RobotSDK_Callbacks_Documentation.md`

**内容包括**:
- 所有回调接口的详细说明
- 回调方法签名和参数
- 回调使用示例和最佳实践
- 线程安全和生命周期管理
- 错误处理指南

**适用于**: 需要处理机器人事件和状态变化的开发者

### 🚀 3. 快速开始指南
**文件**: `RobotSDK_QuickStart_Guide.md`

**内容包括**:
- SDK集成步骤
- 基本功能示例代码
- 传感器事件处理
- 动作控制实现
- 天线和表情控制
- 完整的演示应用

**适用于**: 刚开始使用Robot SDK的开发者

## SDK功能概览

### 🤖 核心功能

| 功能模块 | 主要类/接口 | 描述 |
|---------|------------|------|
| **机器人服务** | `RobotService` | SDK的核心服务类，提供单例访问 |
| **AIDL接口** | `ILetianpaiService` | 底层机器人服务接口 |
| **传感器管理** | `SensorCallback` | 处理触摸、倾斜、TOF等传感器事件 |
| **动作控制** | `ActionMessage` | 控制机器人移动、转向等动作 |
| **天线控制** | `AntennaMessage`, `AntennaLightMessage` | 控制天线运动和灯光效果 |
| **语音播放** | TTS相关方法 | 文字转语音播放 |
| **表情控制** | 表情相关方法 | 控制机器人面部表情 |

### 📡 回调系统

| 回调接口 | 用途 | 关键方法 |
|---------|------|---------|
| `LtpAppCmdCallback` | 应用命令回调 | `onAppCommandReceived` |
| `LtpTTSCallback` | TTS状态回调 | `onTTSCommand` |
| `LtpExpressionCallback` | 表情变化回调 | `onExpressionChanged` |
| `LtpSensorResponseCallback` | 传感器响应回调 | `onSensorResponseReceived` |
| `LtpMcuCommandCallback` | MCU命令回调 | `onMcuCommandReceived` |
| `LtpBleCallback` | 蓝牙回调 | `onBleCommandReceived` |

### 🎮 预定义动作常量

| 动作 | 常量值 | 说明 |
|------|-------|------|
| 前进 | `"63"` | `MOVE_FORWARD` |
| 后退 | `"64"` | `WALK_BACKWARD` |
| 左转 | `"5"` | `TURN_LEFT` |
| 右转 | `"6"` | `GO_RIGHT` |
| 向左转 | `"3"` | `TURN_TO_THE_LEFT` |
| 向右转 | `"4"` | `TURN_TO_THE_RIGHT` |
| 左摇腿 | `"7"` | `LEFT_SHAKING_LEG` |

## 开发流程建议

### 1. 新手开发者
1. 📖 阅读 `RobotSDK_QuickStart_Guide.md`
2. 🏗️ 按照指南集成SDK到项目
3. 🧪 运行基本示例代码
4. 🔧 根据需求定制功能

### 2. 有经验的开发者
1. 📚 快速浏览 `RobotSDK_API_Documentation.md`
2. 🎯 直接查看相关的API接口
3. 📞 根据需要参考 `RobotSDK_Callbacks_Documentation.md`
4. 🚀 开始开发自定义功能

### 3. 系统集成者
1. 📋 全面阅读API文档了解所有功能
2. 🔄 重点关注回调接口和生命周期管理
3. 🛠️ 设计完整的错误处理和恢复机制
4. 📊 实现监控和日志系统

## 代码示例索引

### 基础操作
- **初始化SDK**: 快速开始指南 → 初始化SDK
- **传感器监听**: 快速开始指南 → 传感器事件处理
- **基本动作**: 快速开始指南 → 机器人动作控制

### 高级功能
- **回调处理**: 回调文档 → 回调使用示例
- **错误处理**: 回调文档 → 回调管理最佳实践
- **生命周期管理**: 回调文档 → 回调注册时机

### 完整应用
- **演示应用**: 快速开始指南 → 完整的使用示例
- **舞蹈序列**: 快速开始指南 → performDanceRoutine方法

## 常见使用场景

### 🎮 游戏控制
```java
// 使用传感器触发游戏动作
sensorCallback.onTapResponse() → 游戏开始
sensorCallback.onDoubleTapResponse() → 特殊技能
actionController.moveForward() → 角色移动
```

### 🏠 智能家居助手
```java
// 语音播报 + 表情显示
expressionController.speakWithExpression("温度是25度", "happy");
antennaController.setAntennaLight(0x00FF00); // 绿灯表示正常
```

### 🎭 互动娱乐
```java
// 复杂的互动序列
performDanceRoutine(); // 舞蹈表演
antennaController.antennaWave(); // 挥手致意
expressionController.showHappyExpression(); // 快乐表情
```

### 🔧 机器人监控
```java
// 使用多个回调监控机器人状态
LtpRobotStatusCallback → 监控整体状态
LtpSensorResponseCallback → 监控传感器数据
LtpMcuCommandCallback → 监控硬件状态
```

## 故障排除指南

### 常见问题
1. **服务连接失败** → 检查机器人服务是否运行
2. **回调未触发** → 确认回调注册和生命周期管理
3. **动作无响应** → 检查电机是否开启，命令参数是否正确
4. **权限被拒绝** → 确认必要权限已授予

### 调试技巧
1. **启用详细日志** → 在所有关键方法中添加Log输出
2. **检查AIDL连接** → 验证`robotService.getAIDL()`是否为null
3. **监控异常** → 捕获并记录所有RemoteException
4. **验证参数** → 确认所有命令参数在有效范围内

## 性能优化建议

### 内存管理
- ✅ 及时注销不需要的回调
- ✅ 在Activity/Service生命周期中正确管理资源
- ❌ 避免在回调中创建大量对象

### 响应性能
- ✅ 在回调中使用异步处理避免阻塞
- ✅ 合理使用Handler和线程池
- ❌ 避免在UI线程中执行耗时操作

### 网络和通信
- ✅ 实现重连机制处理连接中断
- ✅ 添加超时处理避免无限等待
- ❌ 避免频繁的服务绑定/解绑操作

## 版本更新说明

### 当前版本: 2.5
- 基于AAR包反编译分析
- 支持完整的机器人控制功能
- 提供丰富的回调接口
- 包含预定义的动作和常量

### 注意事项
- 本文档基于反编译分析，部分方法名称可能与实际略有差异
- 建议结合官方文档和实际测试进行开发
- 如发现文档错误或遗漏，请及时反馈更新

## 联系和支持

如果您在使用过程中遇到问题或需要技术支持，建议：

1. 📖 首先查阅相关文档章节
2. 🔍 检查示例代码是否正确实现
3. 🧪 使用简单的测试案例隔离问题
4. 📝 记录详细的错误信息和复现步骤

---

**文档生成时间**: 2025年7月10日  
**SDK版本**: 2.5  
**文档版本**: 1.0  

*本文档集合为Robot SDK提供了全面的开发指南，帮助开发者快速上手并深入使用机器人控制功能。*
