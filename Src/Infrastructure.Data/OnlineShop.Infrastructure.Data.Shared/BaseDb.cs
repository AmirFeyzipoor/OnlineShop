using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Infrastructure.Data.Shared;

public class BaseDb : DbContext
{
    public BaseDb(DbContextOptions options): base(options)
    {
        
    }
}