using AnalogClock.Controls;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Media;
using Verdure.Braincase.ViewModels;
using Verdure.Common;

namespace Verdure.Braincase;

public partial class MainPage : ContentPage
{
	int count = 0;
    HubConnection connection;

    MainViewModel vm => BindingContext as MainViewModel;

    public MainPage(MainViewModel vm)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5241/Chat")
            .Build();

        connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };
    }

    private async void MainPage_OnLoaded(object sender, EventArgs e)
    {
        //await TextToSpeech.SpeakAsync("你好世界");
        var ret = vm.Init();
        connection.On<MsgModel>("ReceiveMessage", async (msgModel) =>
        {
            if (msgModel != null)
            {
                vm.ChangeClockView(msgModel);

                await TextToSpeech.SpeakAsync(msgModel.TtsText);
            }
        });

        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
        }
        
    }
}

