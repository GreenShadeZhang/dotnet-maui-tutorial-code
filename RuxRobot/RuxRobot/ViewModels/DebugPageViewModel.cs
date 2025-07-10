using MauiApp1.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.ViewModels;

public class DebugPageViewModel : INotifyPropertyChanged
{
    private readonly IRobotControlService _robotService;
    private readonly ILogger<DebugPageViewModel> _logger;
    private readonly ObservableCollection<string> _logMessages = new();
    private bool _isConnected;
    private bool _isTesting;

    public DebugPageViewModel(IRobotControlService robotService, ILogger<DebugPageViewModel> logger)
    {
        _robotService = robotService;
        _logger = logger;
        
        // 初始化命令
        TestForwardActionsCommand = new Command(async () => await TestForwardActionsAsync(), () => IsConnected && !IsTesting);
        TestParameterCombinationsCommand = new Command(async () => await TestParameterCombinationsAsync(), () => IsConnected && !IsTesting);
        TestSingleActionCommand = new Command<string>(async (actionNumber) => await TestSingleActionAsync(actionNumber), (actionNumber) => IsConnected && !IsTesting);
        ClearLogCommand = new Command(() => _logMessages.Clear());
        InitializeRobotCommand = new Command(async () => await InitializeRobotAsync(), () => !IsConnected);
        
        AddLogMessage("🔧 调试页面已加载");
    }

    #region 属性
    public ObservableCollection<string> LogMessages => _logMessages;

    public bool IsConnected
    {
        get => _isConnected;
        set
        {
            _isConnected = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ConnectionStatusText));
            OnPropertyChanged(nameof(ConnectionStatusColor));
            ((Command)TestForwardActionsCommand).ChangeCanExecute();
            ((Command)TestParameterCombinationsCommand).ChangeCanExecute();
            ((Command)InitializeRobotCommand).ChangeCanExecute();
        }
    }

    public bool IsTesting
    {
        get => _isTesting;
        set
        {
            _isTesting = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TestingStatusText));
            OnPropertyChanged(nameof(TestingStatusColor));
            ((Command)TestForwardActionsCommand).ChangeCanExecute();
            ((Command)TestParameterCombinationsCommand).ChangeCanExecute();
        }
    }

    public string ConnectionStatusText => IsConnected ? "✅ 已连接" : "❌ 未连接";
    public Color ConnectionStatusColor => IsConnected ? Colors.Green : Colors.Red;
    
    public string TestingStatusText => IsTesting ? "🧪 测试中..." : "⏸️ 就绪";
    public Color TestingStatusColor => IsTesting ? Colors.Orange : Colors.Gray;
    #endregion

    #region 命令
    public ICommand TestForwardActionsCommand { get; }
    public ICommand TestParameterCombinationsCommand { get; }
    public ICommand TestSingleActionCommand { get; }
    public ICommand ClearLogCommand { get; }
    public ICommand InitializeRobotCommand { get; }
    #endregion

    #region 方法
    private async Task InitializeRobotAsync()
    {
        try
        {
            AddLogMessage("🔄 初始化机器人服务...");
            var result = await _robotService.InitializeAsync();
            
            if (result)
            {
                IsConnected = true;
                AddLogMessage("✅ 机器人服务初始化成功");
            }
            else
            {
                AddLogMessage("❌ 机器人服务初始化失败");
            }
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 初始化错误: {ex.Message}");
        }
    }

    private async Task TestForwardActionsAsync()
    {
        IsTesting = true;
        try
        {
            AddLogMessage("🧪 开始测试多种前进动作编号...");
            await _robotService.DebugTestForwardActionsAsync();
            AddLogMessage("✅ 前进动作测试完成");
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 测试错误: {ex.Message}");
        }
        finally
        {
            IsTesting = false;
        }
    }

    private async Task TestParameterCombinationsAsync()
    {
        IsTesting = true;
        try
        {
            AddLogMessage("🧪 开始测试参数组合...");
            await _robotService.DebugTestParameterCombinationsAsync();
            AddLogMessage("✅ 参数组合测试完成");
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 测试错误: {ex.Message}");
        }
        finally
        {
            IsTesting = false;
        }
    }

    private async Task TestSingleActionAsync(string actionNumberStr)
    {
        if (!int.TryParse(actionNumberStr, out int actionNumber))
        {
            AddLogMessage($"❌ 无效的动作编号: {actionNumberStr}");
            return;
        }

        IsTesting = true;
        try
        {
            AddLogMessage($"🧪 测试单个动作编号: {actionNumber}");
            await _robotService.PerformActionAsync(actionNumber, 50, 1);
            AddLogMessage($"✅ 动作 {actionNumber} 测试完成");
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 测试错误: {ex.Message}");
        }
        finally
        {
            IsTesting = false;
        }
    }

    private async Task TestSpecificParameterOrderAsync()
    {
        IsTesting = true;
        try
        {
            AddLogMessage("🔍 确认具体有效的参数组合...");
            
            // 如果您已经知道哪个组合有效，请告诉我
            // 这里我先测试最可能的组合2: Set(speed, steps, actionNumber)
            AddLogMessage("测试组合2: Set(50, 1, 63) - speed, steps, actionNumber");
            await _robotService.DebugTestParameterCombinationsAsync();
            
            AddLogMessage("✅ 请观察机器人反应并记录哪个组合有效");
        }
        catch (Exception ex)
        {
            AddLogMessage($"❌ 测试错误: {ex.Message}");
        }
        finally
        {
            IsTesting = false;
        }
    }

    private void AddLogMessage(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        _logMessages.Insert(0, $"[{timestamp}] {message}");
        
        // 限制日志条数
        while (_logMessages.Count > 50)
        {
            _logMessages.RemoveAt(_logMessages.Count - 1);
        }
    }
    #endregion

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
