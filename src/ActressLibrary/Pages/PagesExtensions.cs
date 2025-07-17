using ActressLibrary;

namespace ActressLibrary.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<DetailPage>();

        return builder;
    }
}
