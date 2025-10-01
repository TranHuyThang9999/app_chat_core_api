using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.Channels.Commands;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChannelsController : ControllerBaseCustom
{
    private readonly IMediator _mediator;

    public ChannelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("dm")]
    public async Task<IActionResult> CreateDirectMessage([FromBody] CreateDirectMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return OK(result);
    }
}