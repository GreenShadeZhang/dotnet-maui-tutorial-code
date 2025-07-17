using ActressLibrary.Pages;
using ActressLibrary.Services;
using ActressLibrary.ViewModels;
using CommunityToolkit.Maui;

namespace ActressLibrary;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        string dbDataPath = FileSystem.AppDataDirectory + @"\data-litedb.db";
        builder.UseMauiApp<App>().ConfigureEssentials().ConfigureServices(dbDataPath).ConfigurePages().ConfigureViewModels().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        return builder.Build();
    }
}