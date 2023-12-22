using MediatR;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Products.Commands.Edit.Contracts.Events;

public class UpdateReadableDbAfterEditProductEventHandler : 
    INotificationHandler<UpdateReadableDbAfterEditProductEvent>
{
    private readonly IQueriesProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReadableDbAfterEditProductEventHandler(
        IQueriesProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateReadableDbAfterEditProductEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.Find(notification.ProductId); 
        
        EditProduct(notification, product);

        _productRepository.Update(product!);

        await _unitOfWork.SaveChangesAsyncForReadableDb();
    }

    private static void EditProduct(UpdateReadableDbAfterEditProductEvent notification, QueriesProduct? product)
    {
        product!.Name = notification.Name;
        product.ManufactureEmail = notification.ManufactureEmail;
        product.ManufacturePhone = notification.ManufacturePhone;
        product.IsAvailable = notification.IsAvailable;
    }
}