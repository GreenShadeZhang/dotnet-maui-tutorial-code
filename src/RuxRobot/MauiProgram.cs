using Microsoft.Extensions.Logging;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using MauiApp1.Views;

#if ANDROID
using MauiApp1.Platforms.Android.Services;
#endif

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // 注册服务
            RegisterServices(builder.Services);

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // 注册机器人控制服务
#if ANDROID
            services.AddSingleton<IRobotControlService, AndroidRobotControlService>();
#else
            services.AddSingleton<IRobotControlService, DefaultRobotControlService>();
#endif

            // 注册ViewModels
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<DebugPageViewModel>();

            // 注册Pages
            services.AddTransient<MainPage>();
            services.AddTransient<DebugPage>();
        }
    }
}
