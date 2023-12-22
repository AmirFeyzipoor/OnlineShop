using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.UseCases.Products.Commands.Add.Contracts;

namespace OnlineShop.RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator, 
        IHttpContextAccessor accessor)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<int> Add(AddProductCommand command)
    {
        return await _mediator.Send(command);
    }
}