using MediatR;

namespace OnlineShop.UseCases.Products.Commands.Delete.Contracts;

public class DeleteProductCommand : IRequest
{
    public DeleteProductCommand(int productId)
    {
        ProductId = productId;
    }
    
    public int ProductId { get; set; }
}