using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts;

namespace OnlineShop.RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<string> RegisterUser(RegisterUserCommand command)
    {
        return await _mediator.Send(command);
    }
}