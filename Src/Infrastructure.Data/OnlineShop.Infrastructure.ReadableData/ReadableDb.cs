using Microsoft.EntityFrameworkCore;
using OnlineShop.Infrastructure.Data.Shared;

namespace OnlineShop.Infrastructure.ReadableData;

public class ReadableDb : BaseDb
{
    public ReadableDb(string connectionString)
        : this(new DbContextOptionsBuilder<ReadableDb>()
            .UseSqlServer(connectionString).Options)
    {
    }
    
    private ReadableDb(DbContextOptions<ReadableDb> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ReadableDb).Assembly);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}