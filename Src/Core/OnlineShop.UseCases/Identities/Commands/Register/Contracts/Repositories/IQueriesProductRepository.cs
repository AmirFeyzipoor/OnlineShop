using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Infrastructures;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

namespace OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;

public interface IQueriesProductRepository : Repository
{
    Task Add(QueriesProduct product);
    Task<List<GetAllProductDto>> GetAll(GetAllProductFilterDto filter);
}