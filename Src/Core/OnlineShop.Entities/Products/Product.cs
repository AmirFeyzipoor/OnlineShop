using OnlineShop.Entities.Identities;

namespace OnlineShop.Entities.Products;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string? ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
    public User Registrant { get; set; }
    public string RegistrantId { get; set; }
}