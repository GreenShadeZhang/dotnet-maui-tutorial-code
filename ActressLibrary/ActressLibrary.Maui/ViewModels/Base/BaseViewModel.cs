using CommunityToolkit.Mvvm.ComponentModel;

namespace ActressLibrary.Maui.ViewModels;

public partial class BaseViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}