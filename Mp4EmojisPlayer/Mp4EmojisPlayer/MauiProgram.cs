using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Mp4EmojisPlayer.Services;

namespace Mp4EmojisPlayer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<EmotionService>();
            builder.Services.AddTransient<MainPage>();
            
#if ANDROID
            builder.Services.AddSingleton<IVideoService, Platforms.Android.Services.SimpleVideoService>();
#elif WINDOWS
            builder.Services.AddSingleton<IVideoService, Platforms.Windows.Services.VideoService>();
#endif

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
