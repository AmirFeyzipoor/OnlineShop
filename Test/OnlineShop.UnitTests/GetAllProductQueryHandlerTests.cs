using Moq;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;
using OnlineShop.UseCases.Products.Queries.GetAll;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

namespace OnlineShop.UnitTests;

public class GetAllProductQueryHandlerTests
{
    private readonly Mock<IQueriesProductRepository> _mockRepository;
    private readonly GetAllProductQueryHandler _handler;

    public GetAllProductQueryHandlerTests()
    {
        _mockRepository = new Mock<IQueriesProductRepository>();
        _handler = new GetAllProductQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_get_all_products_properly()
    {
        var filter = new GetAllProductFilterDto();
        var query = new GetAllProductQuery(filter);
        
        await _handler.Handle(query, CancellationToken.None);
        
         _mockRepository.Verify(_ => _.GetAll(filter));
    }
}