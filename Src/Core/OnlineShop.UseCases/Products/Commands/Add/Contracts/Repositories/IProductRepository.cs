using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Products.Commands.Add.Contracts.Repositories;

public interface IProductRepository : Repository
{
    Task<bool> IsExist(string manufactureEmail, DateTime produceDate);
    Task Add(Product product);
    Task<Product?> Find(int id);
    void Delete(Product product);
    void Update(Product product);
}