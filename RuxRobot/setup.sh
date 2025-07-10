#!/bin/bash

# Robot SDK MAUI Demo 项目设置脚本
# 用于快速配置开发环境和验证项目结构

echo "🤖 Robot SDK MAUI Demo 项目设置"
echo "================================="

# 检查必需的工具
echo "📋 检查开发环境..."

# 检查 .NET SDK
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    echo "✅ .NET SDK: $DOTNET_VERSION"
else
    echo "❌ .NET SDK 未安装"
    echo "   请从 https://dotnet.microsoft.com/download 下载安装"
    exit 1
fi

# 检查项目文件
echo ""
echo "📁 检查项目结构..."

PROJECT_FILES=(
    "MauiApp1.sln"
    "MauiApp1/MauiApp1.csproj"
    "RobotSDK.Android.Binding/RobotSDK.Android.Binding.csproj"
    "RobotSdk-release-2.5.aar"
)

for file in "${PROJECT_FILES[@]}"; do
    if [ -f "$file" ]; then
        echo "✅ $file"
    else
        echo "❌ $file (缺失)"
    fi
done

# 检查文档文件
echo ""
echo "📚 检查文档文件..."

DOC_FILES=(
    "RobotSDK_Documentation/README.md"
    "RobotSDK_Documentation/RobotSDK_API_Documentation.md"
    "RobotSDK_Documentation/RobotSDK_Callbacks_Documentation.md"
    "RobotSDK_Documentation/RobotSDK_QuickStart_Guide.md"
    "RobotSDK_Documentation/MAUI_Demo_Guide.md"
)

for file in "${DOC_FILES[@]}"; do
    if [ -f "$file" ]; then
        echo "✅ $file"
    else
        echo "❌ $file (缺失)"
    fi
done

# 恢复NuGet包
echo ""
echo "📦 恢复NuGet包..."
dotnet restore

if [ $? -eq 0 ]; then
    echo "✅ NuGet包恢复成功"
else
    echo "❌ NuGet包恢复失败"
    exit 1
fi

# 尝试编译项目
echo ""
echo "🔨 编译项目..."
dotnet build --configuration Debug --verbosity quiet

if [ $? -eq 0 ]; then
    echo "✅ 项目编译成功"
else
    echo "❌ 项目编译失败"
    echo "   请检查错误信息并修复问题"
    exit 1
fi

# 显示下一步操作
echo ""
echo "🎉 项目设置完成！"
echo ""
echo "📋 下一步操作："
echo "1. 在Visual Studio 2022中打开 MauiApp1.sln"
echo "2. 选择Android设备或模拟器"
echo "3. 运行项目开始演示"
echo ""
echo "📚 文档指南："
echo "- 快速开始: RobotSDK_Documentation/RobotSDK_QuickStart_Guide.md"
echo "- API文档:  RobotSDK_Documentation/RobotSDK_API_Documentation.md"
echo "- MAUI指南: RobotSDK_Documentation/MAUI_Demo_Guide.md"
echo ""
echo "🤖 享受Robot SDK开发之旅！"
