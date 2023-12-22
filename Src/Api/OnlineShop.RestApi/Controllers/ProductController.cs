using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.UseCases.Products.Commands.Add.Contracts;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts;
using OnlineShop.UseCases.Products.Commands.Edit.Contracts;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

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

    [HttpGet]
    [AllowAnonymous]
    public async Task<List<GetAllProductDto>> GetAll(
        [FromQuery] GetAllProductFilterDto filter)
    {
        return await _mediator.Send(new GetAllProductQuery(filter));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task Edit(int id, EditProductCommand command)
    {
        command.SetProductId(id);
        
        await _mediator.Send(command);
    }
}