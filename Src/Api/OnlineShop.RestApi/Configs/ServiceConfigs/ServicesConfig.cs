using Autofac;
using Autofac.Extensions.DependencyInjection;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.WritableData;
using OnlineShop.RestApi.Configs.ServiceConfigs.ServicesPrerequisites;
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
        }
    }
}