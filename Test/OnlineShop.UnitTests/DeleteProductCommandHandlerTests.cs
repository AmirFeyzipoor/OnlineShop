using System.Security.Claims;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Infrastructures;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Repositories;
using OnlineShop.UseCases.Products.Commands.Delete;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts.Events;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts.Exceptions;

namespace OnlineShop.UnitTests;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _handler = new DeleteProductCommandHandler(
            _mockRepository.Object,
            _mockUnitOfWork.Object,
            _mockMediator.Object,
            _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task Handle_delete_product_properly()
    {
        var product = new Product { Id = 2, RegistrantId = "d03b9e76-7f83-4594-82bf-1cd838785d15"};
        var command = new DeleteProductCommand(product.Id);

        _mockRepository.Setup(_ => 
                _.Find(It.Is<int>(_ => _ == product.Id)))
            .ReturnsAsync(product);
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, product.RegistrantId) };
        _mockHttpContextAccessor.Setup(_ => _.HttpContext!.User.Claims).Returns(claims);
        
        await _handler.Handle(command, CancellationToken.None);
        
        _mockRepository.Verify(_ => 
            _.Delete(It.Is<Product>(_ => _ == product)));
        _mockMediator.Verify(_ => _.Publish(
            It.IsAny<UpdateReadableDbAfterDeleteProductEvent>(),
            It.IsAny<CancellationToken>()));
        _mockUnitOfWork.Verify(_ => _.SaveChangesAsync());
    }
    
    [Fact]
    public async Task Handle_throw_ProductNotFoundException_when_not_found_product()
    {
        var fakeProductId = 2;
        var command = new DeleteProductCommand(fakeProductId);

        _mockRepository.Setup(_ => 
                _.Find(It.Is<int>(_ => _ == fakeProductId)))
            .ReturnsAsync((Product?)null);

        var expected = async () => 
            await _handler.Handle(command, CancellationToken.None);

        await expected.Should().ThrowExactlyAsync<ProductNotFoundException>();
    }
    
    [Fact]
    public async Task Handle_throw_UnauthorizedDeleteProductAccessException_when_user_not_have_access()
    {
        var product = new Product { Id = 2, RegistrantId = "d03b9e76-7f83-4594-82bf-1cd838785d15"};
        var fakeUserId = "fakeUserId";
        var command = new DeleteProductCommand(product.Id);

        _mockRepository.Setup(_ => 
                _.Find(It.Is<int>(_ => _ == product.Id)))
            .ReturnsAsync(product);
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, fakeUserId) };
        _mockHttpContextAccessor.Setup(_ => _.HttpContext!.User.Claims).Returns(claims);
        
        var expected = async () => 
            await _handler.Handle(command, CancellationToken.None);

        await expected.Should().ThrowExactlyAsync<UnauthorizedDeleteProductAccessException>();
    }
}