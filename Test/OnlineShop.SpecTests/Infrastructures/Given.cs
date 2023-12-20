namespace OnlineShop.SpecTests.Infrastructures;

public class Given : Attribute
{
    public Given(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}