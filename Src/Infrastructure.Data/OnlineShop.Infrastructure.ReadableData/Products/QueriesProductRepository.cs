using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;

namespace OnlineShop.Infrastructure.ReadableData.Products;

public class QueriesProductRepository : IQueriesProductRepository
{
    private readonly DbSet<QueriesProduct> _products;

    public QueriesProductRepository(ReadableDb db)
    {
        _products = db.Set<QueriesProduct>();
    }

    public async Task Add(QueriesProduct product)
    {
        await _products.AddAsync(product);
    }
}