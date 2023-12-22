using MediatR;
using OnlineShop.Entities.Products;

namespace OnlineShop.UseCases.Products.Commands.Delete.Contracts.Events;

public class UpdateReadableDbAfterDeleteProductEvent : INotification
{
    public UpdateReadableDbAfterDeleteProductEvent(int productId)
    {
        ProductId = productId;
    }

    public int ProductId { get; set; }
}