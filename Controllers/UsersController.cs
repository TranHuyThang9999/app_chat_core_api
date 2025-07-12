using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.Users.Commands;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = userId }, userId);
    }
}
