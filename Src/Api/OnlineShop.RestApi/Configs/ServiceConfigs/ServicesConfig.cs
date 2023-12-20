using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Entities.Identities;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.ReadableData.Identities;
using OnlineShop.Infrastructure.WritableData;
using OnlineShop.RestApi.Configs.AutoMapperConfigs;
using OnlineShop.RestApi.Configs.ServiceConfigs.ServicesPrerequisites;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts.Repositories;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.RestApi.Configs.ServiceConfigs;

public static class ServicesConfig
{
    private static readonly ConnectionStrings _dbConnectionString = new();

    private static void Initialized(WebApplicationBuilder builder)
    {
        builder.Configuration.Bind("ConnectionStrings", _dbConnectionString);
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        Initialized(builder);
        
        var config = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfig());
        });
        var mapper = config.CreateMapper();
        builder.Services.AddSingleton(mapper);
        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies
                (typeof(RegisterUserCommand).Assembly));
        
        builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Lockout.AllowedForNewUsers = false;
            })
            .AddEntityFrameworkStores<WritableDb>()
            .AddDefaultTokenProviders();

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(b => b
            .RegisterModule(new AutoFacModule()));
    }

    public class AutoFacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WritableDb>()
                .WithParameter("connectionString", _dbConnectionString.WritableDb)
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReadableDb>()
                .WithParameter("connectionString", _dbConnectionString.ReadableDb)
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(
                    typeof(UserQueriesRepository).Assembly)
                .AssignableTo<Repository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}