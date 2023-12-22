using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Infrastructures;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Repositories;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts.Events;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts.Exceptions;

namespace OnlineShop.UseCases.Products.Commands.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _accessor;

    public DeleteProductCommandHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IHttpContextAccessor accessor)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _accessor = accessor;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.Find(request.ProductId);

        StopIfProductNotFound(product);

        var userId = _accessor.HttpContext!.User.Claims
            .FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
        
        StopIfUnauthorizedDeleteProductAccess(product!.RegistrantId, userId);

        _repository.Delete(product);

        await PublishEventForUpdateReadableDb(product.Id);

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task PublishEventForUpdateReadableDb(int productId)
    {
        var updateReadableDbEvent = new UpdateReadableDbAfterDeleteProductEvent(productId);
        await _mediator.Publish(updateReadableDbEvent, CancellationToken.None);
    }

    private static void StopIfUnauthorizedDeleteProductAccess(string registrantId, string? userId)
    {
        if (registrantId != userId)
            throw new UnauthorizedDeleteProductAccessException();
    }

    private static void StopIfProductNotFound(Product? product)
    {
        if (product == null)
            throw new ProductNotFoundException();
    }
}