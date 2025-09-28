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
    /// Lấy danh sách tin nhắn của user
    /// </summary>
    /// <param name="userId">ID của user</param>
    /// <param name="otherUserId">ID của user khác (tùy chọn)</param>
    /// <param name="onlyUnread">Chỉ lấy tin nhắn chưa đọc</param>
    /// <returns>Danh sách tin nhắn</returns>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMessages(
        long userId, 
        [FromQuery] long? otherUserId = null, 
        [FromQuery] bool onlyUnread = false)
    {
        var query = new GetMessagesQuery
        {
            UserId = userId,
            OtherUserId = otherUserId,
            OnlyUnread = onlyUnread
        };

        var result = await _mediator.Send(query);
        return OK(result);
    }

    /// <summary>
    /// Lấy tin nhắn giữa hai user
    /// </summary>
    /// <param name="userId1">ID user thứ nhất</param>
    /// <param name="userId2">ID user thứ hai</param>
    /// <returns>Danh sách tin nhắn giữa hai user</returns>
    [HttpGet("conversation/{userId1}/{userId2}")]
    public async Task<IActionResult> GetConversation(long userId1, long userId2)
    {
        var query = new GetMessagesQuery
        {
            UserId = userId1,
            OtherUserId = userId2,
            OnlyUnread = false
        };

        var result = await _mediator.Send(query);
        return OK(result);
    }

    /// <summary>
    /// Lấy tin nhắn chưa đọc của user
    /// </summary>
    /// <param name="userId">ID của user</param>
    /// <returns>Danh sách tin nhắn chưa đọc</returns>
    [HttpGet("unread/{userId}")]
    public async Task<IActionResult> GetUnreadMessages(long userId)
    {
        var query = new GetMessagesQuery
        {
            UserId = userId,
            OnlyUnread = true
        };

        var result = await _mediator.Send(query);
        return OK(result);
    }
}