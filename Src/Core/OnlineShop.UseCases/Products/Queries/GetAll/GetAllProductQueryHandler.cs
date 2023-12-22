using MediatR;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

namespace OnlineShop.UseCases.Products.Queries.GetAll;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<GetAllProductDto>>
{
    private readonly IQueriesProductRepository _productRepository;

    public GetAllProductQueryHandler(IQueriesProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<GetAllProductDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAll(request.Filter);
    }
}