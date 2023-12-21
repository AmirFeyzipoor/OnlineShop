using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;

public interface IUserQueriesRepository : Repository
{
    Task AddAsync(QueriesUser user);
}