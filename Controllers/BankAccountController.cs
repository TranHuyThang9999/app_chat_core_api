using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Features.BankAccounts.Commands;

namespace PaymentCoreServiceApi.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankAccountController: ControllerBaseCustom
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("create-bank-account")]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return OK(result);
    }

}