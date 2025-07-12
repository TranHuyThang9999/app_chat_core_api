using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.Auth;
using PaymentCoreServiceApi.Features.Auth.Commands;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Yêu cầu authentication mặc định cho controller
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
