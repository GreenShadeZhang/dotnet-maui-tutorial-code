using Verdure.Braincase.ViewModels;

namespace Verdure.Braincase;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		var main =  serviceProvider.GetServices<MainViewModel>().First();
		//var main = new MainViewModel();

		MainPage = new MainPage(main);
	}
}
