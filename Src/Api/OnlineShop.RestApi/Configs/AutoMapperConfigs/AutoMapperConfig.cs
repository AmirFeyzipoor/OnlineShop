using AutoMapper;
using OnlineShop.Entities.Identities;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts;
using OnlineShop.UseCases.Products.Commands.Add.Contracts;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Events;

namespace OnlineShop.RestApi.Configs.AutoMapperConfigs;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<AddProductCommand, Product>();
        CreateMap<UpdateReadableDbAfterAddProductEvent, QueriesProduct>();
    }
}