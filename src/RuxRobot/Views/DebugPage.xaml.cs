using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class DebugPage : ContentPage
{
    public DebugPage(DebugPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
