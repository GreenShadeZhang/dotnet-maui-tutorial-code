﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace ActressLibrary.ViewModels;

public partial class BaseViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}