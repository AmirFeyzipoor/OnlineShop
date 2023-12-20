using Microsoft.EntityFrameworkCore;
using OnlineShop.Infrastructure.Data.Shared;

namespace OnlineShop.Infrastructure.WritableData;

public class WritableDb : BaseDb
{
    public WritableDb(string connectionString)
        : this(new DbContextOptionsBuilder<WritableDb>()
            .UseSqlServer(connectionString).Options)
    {
    }

    private WritableDb(DbContextOptions<WritableDb> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(WritableDb).Assembly);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}