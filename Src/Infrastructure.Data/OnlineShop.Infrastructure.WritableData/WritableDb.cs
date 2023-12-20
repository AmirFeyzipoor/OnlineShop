using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Identities;

namespace OnlineShop.Infrastructure.WritableData;

public class WritableDb : IdentityDbContext<
    User,
    IdentityRole,
    string,
    UserClaim,
    IdentityUserRole<string>,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>>
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