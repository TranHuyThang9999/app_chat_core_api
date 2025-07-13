using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Common;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
public abstract class ControllerBaseCustom : ControllerBase
{
    protected IActionResult OK<T>(ApiResponse<T> response)
    {
        return StatusCode(response.HttpStatus, response);
    }
}