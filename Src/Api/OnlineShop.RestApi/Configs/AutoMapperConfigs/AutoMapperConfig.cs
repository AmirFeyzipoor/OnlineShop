using AutoMapper;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts;

namespace OnlineShop.RestApi.Configs.AutoMapperConfigs;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<RegisterUserCommand, User>().ReverseMap();
    }
}