using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Identities.Commands.Add.Contracts.Repositories;

public interface IUserQueriesRepository : Repository
{
    Task AddAsync(QueriesUser user);
}