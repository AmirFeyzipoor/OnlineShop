using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Entities.Identities;

public class User : IdentityUser<string>
{
    public DateTime CreationDate { get; set; }
}