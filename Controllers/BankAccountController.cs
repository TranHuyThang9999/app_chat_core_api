using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.BankAccounts.Commands;

namespace PaymentCoreServiceApi.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankAccountController: BaseController
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("create-bank-account")]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountCommand command)
    {
        try
        {
            var bankAccount = await _mediator.Send(command);
            return SuccessResponse(bankAccount);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex.Message, 400);
        }
    }

}