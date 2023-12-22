using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShop.Entities.Identities;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.ReadableData.Identities;
using OnlineShop.Infrastructure.WritableData;
using OnlineShop.Infrastructure.WritableData.Products;
using OnlineShop.RestApi.Configs.AutoMapperConfigs;
using OnlineShop.RestApi.Configs.ServiceConfigs.ServicesPrerequisites;
using OnlineShop.UseCases.Identities.Commands.Login;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts.TokenConfigs;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.RestApi.Configs.ServiceConfigs;

public static class ServicesConfig
{
    private static readonly ConnectionStrings _dbConnectionString = new();
    private static readonly JwtBearerTokenSettings _jwtBearerTokenSettings = new();

    private static void Initialized(WebApplicationBuilder builder)
    {
        builder.Configuration.Bind("ConnectionStrings", _dbConnectionString);
        builder.Configuration.Bind("JwtBearerTokenSettings", _jwtBearerTokenSettings);
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
        
        builder.Services.Configure<JwtBearerTokenSettings>(
            builder.Configuration.GetSection("JwtBearerTokenSettings"));
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Example: \"Bearer token\""
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _jwtBearerTokenSettings.Audience,
                    ValidIssuer = _jwtBearerTokenSettings.Issuer,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerTokenSettings.SecretKey))
                };
            });

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
            
            builder.RegisterAssemblyTypes(
                    typeof(ProductRepository).Assembly)
                .AssignableTo<Repository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}