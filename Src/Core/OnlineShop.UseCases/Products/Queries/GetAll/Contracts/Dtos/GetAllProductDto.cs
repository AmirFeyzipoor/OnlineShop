namespace OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

public class GetAllProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string? ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
    public string RegistrantId { get; set; }
    public string RegistrantUserName { get; set; }
}