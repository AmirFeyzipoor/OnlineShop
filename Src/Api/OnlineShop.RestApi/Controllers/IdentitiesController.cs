using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts;

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

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<string> RegisterUser(RegisterUserCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        return Ok(new
        {
            AccessToken = await _mediator.Send(command)
        });
    }
}