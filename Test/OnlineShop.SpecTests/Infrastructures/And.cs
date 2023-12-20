namespace OnlineShop.SpecTests.Infrastructures;

[AttributeUsage(
    AttributeTargets.Method, 
    AllowMultiple = true, 
    Inherited = true)]
public class And : Attribute
{
    public And(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}