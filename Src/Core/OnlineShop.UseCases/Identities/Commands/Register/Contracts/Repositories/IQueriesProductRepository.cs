using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;

public interface IQueriesProductRepository : Repository
{
    Task Add(QueriesProduct product);
}