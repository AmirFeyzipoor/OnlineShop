using AutoMapper;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Add;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts;

namespace OnlineShop.RestApi.Configs.AutoMapperConfigs;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<RegisterUserCommand, User>().ReverseMap();
    }
}