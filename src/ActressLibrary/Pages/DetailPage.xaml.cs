using ActressLibrary.ViewModels;

namespace ActressLibrary.Pages;

public partial class DetailPage : ContentPage
{
    DetailViewModel vm => BindingContext as DetailViewModel;
    public DetailPage(DetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
    }

}