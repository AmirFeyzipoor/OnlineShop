namespace OnlineShop.SpecTests.Infrastructures;

public class Scenario : Attribute
{
    public Scenario(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
}