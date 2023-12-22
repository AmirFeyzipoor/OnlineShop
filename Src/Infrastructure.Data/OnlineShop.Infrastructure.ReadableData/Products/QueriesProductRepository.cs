using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

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

    public async Task<List<GetAllProductDto>> GetAll(GetAllProductFilterDto filter)
    {
        var products = _products.Select(_ => new GetAllProductDto()
        {
            Id = _.Id,
            Name = _.Name,
            ManufactureEmail = _.ManufactureEmail,
            ManufacturePhone = _.ManufacturePhone,
            ProduceDate = _.ProduceDate,
            RegistrantUserName = _.RegistrantUserName,
            RegistrantId = _.RegistrantId,
            IsAvailable = _.IsAvailable
        });
        
        
        if (!filter.RegistrantUserName.IsNullOrEmpty())
        {
            products = products.Where(_ => _.RegistrantUserName.Contains(filter.RegistrantUserName!));
        }
        
        if (!filter.RegistrantId.IsNullOrEmpty())
        {
            products = products.Where(_ => _.RegistrantId == filter.RegistrantId);
        }

        return await products.ToListAsync();
    }
}