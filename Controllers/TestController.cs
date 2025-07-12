using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public TestController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    [HttpGet("current-user")]
    public IActionResult GetCurrentUserInfo()
    {
        var userInfo = new
        {
            UserId = _currentUser.Id,
            Username = _currentUser.UserName,
            Email = _currentUser.Email,
            IsAuthenticated = _currentUser.IsAuthenticated
        };

        return Ok(userInfo);
    }

    [HttpGet("protected-resource")]
    public IActionResult GetProtectedResource()
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            Message = $"This is protected data for user: {_currentUser.UserName}",
            UserId = _currentUser.Id,
            Timestamp = DateTime.UtcNow
        });
    }
}
