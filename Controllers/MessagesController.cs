using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.Messages.Commands;
using PaymentCoreServiceApi.Features.Messages.Queries;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBaseCustom
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gửi tin nhắn mới
    /// </summary>
    /// <param name="command">Thông tin tin nhắn</param>
    /// <returns>Tin nhắn đã gửi</returns>
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return OK(result);
    }
    
    /// <summary>
    /// Lấy tin nhắn trong cuộc trò chuyện
    /// </summary>
    /// <param name="query">Thông tin cuộc trò chuyện</param>
    /// <returns>Danh sách tin nhắn</returns>
    [HttpGet("conversation")]
    public async Task<IActionResult> GetMessages([FromQuery] GetMessagesQuery query)
    {
        var result = await _mediator.Send(query);
        return OK(result);
    }
}