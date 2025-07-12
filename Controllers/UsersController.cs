using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.Users.Commands;
using PaymentCoreServiceApi.Features.Users.Queries;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Yêu cầu authentication cho tất cả các endpoints trong controller
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous] // Cho phép tạo user mà không cần authentication
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return Ok(user);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _mediator.Send(new GetUserProfileQuery());
        return Ok(profile);
    }
}
