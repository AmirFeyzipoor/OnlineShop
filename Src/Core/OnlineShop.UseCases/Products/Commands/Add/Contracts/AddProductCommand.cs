using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineShop.UseCases.Products.Commands.Add.Contracts;

public class AddProductCommand : IRequest<int>
{
    public AddProductCommand(
        string name,
        string manufactureEmail,
        DateTime produceDate)
    {
        ManufactureEmail = manufactureEmail;
        ProduceDate = produceDate;
        Name = name;
    }

    [Required]
    public DateTime ProduceDate { get; set; }
    [Required]
    [EmailAddress]
    public string ManufactureEmail { get; set; }
    [Required]
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
    public string? ManufacturePhone { get; set; }
}