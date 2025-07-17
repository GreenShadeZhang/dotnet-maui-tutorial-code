using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Verdure.Braincase.Api.Models;
using Verdure.Braincase.Api.Services;

namespace Verdure.Braincase.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SendMsgController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;


    private readonly ILogger<SendMsgController> _logger;

    public SendMsgController(ILogger<SendMsgController> logger, IHubContext<ChatHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }


    [HttpPost(Name = "SendMsg")]
    public async Task SendMsgAsync(MsgModel msgModel)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", msgModel);
    }
}