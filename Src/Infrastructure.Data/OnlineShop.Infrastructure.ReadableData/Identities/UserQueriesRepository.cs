using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts.Repositories;

namespace OnlineShop.Infrastructure.ReadableData.Identities;

public class UserQueriesRepository : IUserQueriesRepository
{
    private readonly DbSet<QueriesUser> _users;

    public UserQueriesRepository(ReadableDb db)
    {
        _users = db.Set<QueriesUser>();
    }

    public async Task AddAsync(QueriesUser user)
    {
        await _users.AddAsync(user);
    }
}