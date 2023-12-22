using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using OnlineShop.Entities.Identities;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.WritableData;
using OnlineShop.Infrastructure.WritableData.Products;
using OnlineShop.UseCases.Identities.Commands.Register;
using OnlineShop.UseCases.Infrastructures;
using OnlineShop.UseCases.Products.Commands.Add;

namespace OnlineShop.TestTools.Products;

public static class ProductFactory
{
    public static AddProductCommandHandler GenerateAddProductCommandHandler(
        WritableDb writableDb,
        ReadableDb readableDb,
        Mock<IMapper> mockMapper,
        Mock<IMediator> mockMediator,
        Mock<UserManager<User>> mockUserManager,
        Mock<IHttpContextAccessor> mockHttpContextAccessor)
    {
        var productRepository = new ProductRepository(writableDb);
        var unitOfWork = new UnitOfWork(writableDb, readableDb);
        return new AddProductCommandHandler(
            mockMapper.Object,
            productRepository,
            unitOfWork,
            mockMediator.Object,
            mockUserManager.Object,
            mockHttpContextAccessor.Object);
    }
}