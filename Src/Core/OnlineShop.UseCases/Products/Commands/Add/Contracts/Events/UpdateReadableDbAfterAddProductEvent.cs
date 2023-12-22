using MediatR;

namespace OnlineShop.UseCases.Products.Commands.Add.Contracts.Events;

public class UpdateReadableDbAfterAddProductEvent : INotification
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufactureEmail { get; set; }
    public string RegistrantId { get; set; }
    public string RegistrantUserName { get; set; }
    public bool IsAvailable { get; set; }
    public string? ManufacturePhone { get; set; } 
}