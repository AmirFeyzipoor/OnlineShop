using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineShop.UseCases.Products.Commands.Edit.Contracts;

public class EditProductCommand : IRequest
{
    [Required]
    [EmailAddress]
    public string ManufactureEmail { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    public bool IsAvailable { get; set; }
    public string? ManufacturePhone { get; set; }

    private int _productId;

    public void SetProductId(int productId)
    {
        _productId = productId;
    }
    
    public int GetProductId()
    {
        return _productId;
    }
}