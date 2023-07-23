using System;
using AnalogClock.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Verdure.Braincase.Services.ClockView;
using Verdure.Common;

namespace Verdure.Braincase.ViewModels
{
	public partial class MainViewModel :ObservableObject
    {
        private readonly IClockViewProviderFactory _clockViewProviderFactory;
        public MainViewModel(IClockViewProviderFactory clockViewProviderFactory)
		{
            _clockViewProviderFactory = clockViewProviderFactory;
		}

        [ObservableProperty]
        View _customClock;

        [ObservableProperty]
        string _title = "你好，世界!";

        [ObservableProperty]
        Color _fontColor = Colors.AliceBlue;

        public bool Init()
        {
            var viewProvider = _clockViewProviderFactory.CreateClockViewProvider("DefaultView");
            CustomClock = viewProvider.CreateClockView("DefaultView");

            return true;
        }

        public void ChangeClockView(MsgModel msgModel)
        {
            Title = msgModel.Content;
            var fontColor = Color.FromRgb(msgModel.FontColorR,msgModel.FontColorG,msgModel.FontColorB);
            FontColor = fontColor;
            var viewProvider = _clockViewProviderFactory.CreateClockViewProvider(msgModel.ClockName);
            CustomClock = viewProvider.CreateClockView(msgModel.ClockName);
        }
    }
}

