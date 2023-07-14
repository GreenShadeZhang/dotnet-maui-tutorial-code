using Microsoft.AspNetCore.SignalR;
using Verdure.Common;

namespace Verdure.Braincase.Api.Services
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(MsgModel msgModel)
            => await Clients.All.SendAsync("ReceiveMessage", msgModel);
    }
}
