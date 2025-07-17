using Microsoft.AspNetCore.SignalR;
using Verdure.Braincase.Api.Models;

namespace Verdure.Braincase.Api.Services;

public class ChatHub : Hub
{
    public async Task SendMessage(MsgModel msgModel)
        => await Clients.All.SendAsync("ReceiveMessage", msgModel);
}
