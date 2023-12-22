using MediatR;
using OnlineShop.UseCases.Products.Queries.GetAll.Contracts.Dtos;

namespace OnlineShop.UseCases.Products.Queries.GetAll.Contracts;

public class GetAllProductQuery : IRequest<List<GetAllProductDto>>
{
    public GetAllProductQuery(GetAllProductFilterDto filter)
    {
        Filter = filter;
    }

    public GetAllProductFilterDto Filter { get; set; }
}