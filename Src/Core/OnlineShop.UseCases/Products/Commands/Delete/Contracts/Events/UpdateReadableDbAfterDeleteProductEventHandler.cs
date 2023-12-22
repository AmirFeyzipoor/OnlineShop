using MediatR;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;

namespace OnlineShop.UseCases.Products.Commands.Delete.Contracts.Events;

public class UpdateReadableDbAfterDeleteProductEventHandler : INotificationHandler<UpdateReadableDbAfterDeleteProductEvent>
{
    private readonly IQueriesProductRepository _productRepository;

    public UpdateReadableDbAfterDeleteProductEventHandler(IQueriesProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(UpdateReadableDbAfterDeleteProductEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.Find(notification.ProductId);
        
        _productRepository.Delete(product!);
    }
}