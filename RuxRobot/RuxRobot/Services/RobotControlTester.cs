using Microsoft.Extensions.Logging;
using MauiApp1.Services;

namespace MauiApp1.Services;

/// <summary>
/// 机器人控制测试类，用于验证动作命令的正确性
/// </summary>
public class RobotControlTester
{
    private readonly IRobotControlService _robotService;
    private readonly ILogger<RobotControlTester> _logger;

    public RobotControlTester(IRobotControlService robotService, ILogger<RobotControlTester> logger)
    {
        _robotService = robotService;
        _logger = logger;
    }

    /// <summary>
    /// 测试基础移动动作
    /// </summary>
    public async Task TestBasicMovementAsync()
    {
        _logger.LogInformation("=== 开始测试基础移动动作 ===");

        try
        {
            // 测试前进
            _logger.LogInformation($"测试前进动作 (编号: {RobotActionCommands.MoveForward})");
            await _robotService.MoveForwardAsync(50, 1);
            await Task.Delay(2000);

            // 测试后退
            _logger.LogInformation($"测试后退动作 (编号: {RobotActionCommands.WalkBackward})");
            await _robotService.MoveBackwardAsync(50, 1);
            await Task.Delay(2000);

            // 测试左转
            _logger.LogInformation($"测试左转动作 (编号: {RobotActionCommands.TurnLeft})");
            await _robotService.TurnLeftAsync(50, 1);
            await Task.Delay(2000);

            // 测试右转
            _logger.LogInformation($"测试右转动作 (编号: {RobotActionCommands.TurnRight})");
            await _robotService.TurnRightAsync(50, 1);
            await Task.Delay(2000);

            _logger.LogInformation("=== 基础移动动作测试完成 ===");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "基础移动动作测试失败");
        }
    }

    /// <summary>
    /// 测试不同速度的前进动作
    /// </summary>
    public async Task TestForwardSpeedVariationsAsync()
    {
        _logger.LogInformation("=== 开始测试不同速度的前进动作 ===");

        try
        {
            int[] speeds = { 20, 50, 80 };
            
            foreach (var speed in speeds)
            {
                _logger.LogInformation($"测试前进速度: {speed}");
                await _robotService.MoveForwardAsync(speed, 1);
                await Task.Delay(2000);
            }

            _logger.LogInformation("=== 不同速度前进动作测试完成 ===");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "不同速度前进动作测试失败");
        }
    }

    /// <summary>
    /// 测试步数变化的前进动作
    /// </summary>
    public async Task TestForwardStepVariationsAsync()
    {
        _logger.LogInformation("=== 开始测试不同步数的前进动作 ===");

        try
        {
            int[] steps = { 1, 2, 3 };
            
            foreach (var step in steps)
            {
                _logger.LogInformation($"测试前进步数: {step}");
                await _robotService.MoveForwardAsync(50, step);
                await Task.Delay(3000);
            }

            _logger.LogInformation("=== 不同步数前进动作测试完成 ===");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "不同步数前进动作测试失败");
        }
    }

    /// <summary>
    /// 测试简单的移动序列
    /// </summary>
    public async Task TestMovementSequenceAsync()
    {
        _logger.LogInformation("=== 开始测试移动序列 ===");

        try
        {
            // 创建一个简单的方形移动序列
            var sequence = new[]
            {
                (nameof(_robotService.MoveForwardAsync), (Func<Task>)(() => _robotService.MoveForwardAsync(50, 2))),
                (nameof(_robotService.TurnRightAsync), (Func<Task>)(() => _robotService.TurnRightAsync(50, 1))),
                (nameof(_robotService.MoveForwardAsync), (Func<Task>)(() => _robotService.MoveForwardAsync(50, 2))),
                (nameof(_robotService.TurnRightAsync), (Func<Task>)(() => _robotService.TurnRightAsync(50, 1))),
                (nameof(_robotService.MoveForwardAsync), (Func<Task>)(() => _robotService.MoveForwardAsync(50, 2))),
                (nameof(_robotService.TurnRightAsync), (Func<Task>)(() => _robotService.TurnRightAsync(50, 1))),
                (nameof(_robotService.MoveForwardAsync), (Func<Task>)(() => _robotService.MoveForwardAsync(50, 2))),
                (nameof(_robotService.TurnRightAsync), (Func<Task>)(() => _robotService.TurnRightAsync(50, 1)))
            };

            foreach (var (actionName, action) in sequence)
            {
                _logger.LogInformation($"执行动作: {actionName}");
                await action();
                await Task.Delay(2000);
            }

            _logger.LogInformation("=== 移动序列测试完成 ===");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "移动序列测试失败");
        }
    }

    /// <summary>
    /// 测试电机控制状态
    /// </summary>
    public async Task TestMotorControlAsync()
    {
        _logger.LogInformation("=== 开始测试电机控制 ===");

        try
        {
            // 禁用电机
            _logger.LogInformation("禁用电机");
            await _robotService.DisableMotorAsync();
            await Task.Delay(1000);

            // 尝试移动（应该失败）
            _logger.LogInformation("尝试在电机禁用时移动");
            await _robotService.MoveForwardAsync(50, 1);
            await Task.Delay(1000);

            // 重新启用电机
            _logger.LogInformation("重新启用电机");
            await _robotService.EnableMotorAsync();
            await Task.Delay(1000);

            // 再次尝试移动（应该成功）
            _logger.LogInformation("在电机启用后尝试移动");
            await _robotService.MoveForwardAsync(50, 1);
            await Task.Delay(2000);

            _logger.LogInformation("=== 电机控制测试完成 ===");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "电机控制测试失败");
        }
    }

    /// <summary>
    /// 运行所有测试
    /// </summary>
    public async Task RunAllTestsAsync()
    {
        _logger.LogInformation("🚀 开始运行所有机器人控制测试...");

        await TestBasicMovementAsync();
        await Task.Delay(3000);

        await TestForwardSpeedVariationsAsync();
        await Task.Delay(3000);

        await TestForwardStepVariationsAsync();
        await Task.Delay(3000);

        await TestMotorControlAsync();
        await Task.Delay(3000);

        await TestMovementSequenceAsync();

        _logger.LogInformation("✅ 所有机器人控制测试完成!");
    }
}
