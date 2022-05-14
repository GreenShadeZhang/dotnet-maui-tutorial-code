using ActressLibrary.Maui.Pages;

namespace ActressLibrary.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
    }
}
