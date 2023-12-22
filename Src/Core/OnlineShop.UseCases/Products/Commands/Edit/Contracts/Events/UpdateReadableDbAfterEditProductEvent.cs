using MediatR;

namespace OnlineShop.UseCases.Products.Commands.Edit.Contracts.Events;

public class UpdateReadableDbAfterEditProductEvent : INotification
{
    public string ManufactureEmail { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
    public string? ManufacturePhone { get; set; }
    public int ProductId { get; set; }
}