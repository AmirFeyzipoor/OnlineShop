namespace OnlineShop.SpecTests.Infrastructures;

public class When : Attribute
{
    public When(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}