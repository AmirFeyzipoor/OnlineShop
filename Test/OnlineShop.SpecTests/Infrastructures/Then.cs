namespace OnlineShop.SpecTests.Infrastructures;

public class Then : Attribute
{
    public Then(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}