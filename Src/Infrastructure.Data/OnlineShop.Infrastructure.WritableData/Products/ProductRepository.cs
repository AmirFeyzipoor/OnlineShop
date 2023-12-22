using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Repositories;

namespace OnlineShop.Infrastructure.WritableData.Products;

public class ProductRepository : IProductRepository
{
    private readonly DbSet<Product> _products;

    public ProductRepository(WritableDb db)
    {
        _products = db.Set<Product>();
    }

    public async Task<bool> IsExist(string manufactureEmail, DateTime produceDate)
    {
        var result1 = await _products.AnyAsync(_ =>
            _.ManufactureEmail == manufactureEmail &&
            _.ProduceDate.Year == produceDate.Year &&
            _.ProduceDate.Month == produceDate.Month &&
            _.ProduceDate.Day == produceDate.Day &&
            _.ProduceDate.Hour == produceDate.Hour &&
            _.ProduceDate.Minute == produceDate.Minute &&
            _.ProduceDate.Second == produceDate.Second);

        return result1;
    }

    public async Task Add(Product product)
    {
        await _products.AddAsync(product);
    }

    public async Task<Product?> Find(int id)
    {
        return await _products.FirstOrDefaultAsync(_ => _.Id == id);
    }

    public void Delete(Product product)
    {
        _products.Remove(product);
    }

    public void Update(Product product)
    {
        _products.Update(product);
    }
}