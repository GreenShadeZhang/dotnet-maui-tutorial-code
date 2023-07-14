using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Media;
using Verdure.Common;

namespace Verdure.Braincase;

public partial class MainPage : ContentPage
{
	int count = 0;
    HubConnection connection;

    public MainPage()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);


        connection = new HubConnectionBuilder()
            .WithUrl("http://192.168.3.221:5241/Chat")
            .Build();

        connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };
    }

    private async void MainPage_OnLoaded(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync("你好世界");
        connection.On<MsgModel>("ReceiveMessage", async (msgModel) =>
        {
            if (msgModel != null)
            {
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

