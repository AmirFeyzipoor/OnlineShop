using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using OnlineShop.Entities.Identities;
using OnlineShop.TestTools.Identities;
using OnlineShop.UseCases.Identities.Commands.Register;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Events;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Exceptions;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UnitTests;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMediator> _mockMediator;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        var userStore = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStore.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        
        _handler = IdentityFactory
            .GenerateRegisterUserCommandHandler(_mockUserManager, _mockMediator, _mockUnitOfWork);
    }

    [Fact]
    public async Task Handle_register_user_properly()
    {
        var command = new RegisterUserCommand(userName: "Amir", password: "Amir007");

        _mockUserManager.Setup(_ => _.FindByNameAsync(
                It.Is<string>(_ => _ == command.UserName)))
            .ReturnsAsync((User?)null);
        
        _mockUserManager.Setup(_ => _.CreateAsync(
                It.Is<User>(_ => _.UserName == command.UserName),
                It.Is<string>(_ => _ == command.Password)))
            .ReturnsAsync(IdentityResult.Success);
        
        await _handler.Handle(command,CancellationToken.None);
        
        _mockMediator.Verify(_ => _.Publish(
            It.IsAny<UpdateReadableDbAfterRegisterUserEvent>(),
            It.IsAny<CancellationToken>()));
        
        _mockUnitOfWork.Verify(_ => _.SaveChangesAsync());
    }
    
    [Fact]
    public async Task Handle_throw_exception_when_userName_is_duplicated()
    {
        var command = new RegisterUserCommand(userName: "Amir", password: "Amir007");
        var user = new User()
        {
            UserName = command.UserName
        };

        _mockUserManager.Setup(_ => _.FindByNameAsync(
                It.Is<string>(_ => _ == command.UserName)))
            .ReturnsAsync(user);

        var expected = async () => await _handler.Handle(command,CancellationToken.None);

        await expected.Should().ThrowExactlyAsync<DuplicatedUserNameException>();
    }
    
    [Fact]
    public async Task Handle_throw_exception_when_Create_User_Failed()
    {
        var command = new RegisterUserCommand(userName: "Amir", password: "Amir007");

        _mockUserManager.Setup(_ => _.FindByNameAsync(
                It.Is<string>(_ => _ == command.UserName)))
            .ReturnsAsync((User?)null);
        
        _mockUserManager.Setup(_ => _.CreateAsync(
                It.Is<User>(_ => _.UserName == command.UserName),
                It.Is<string>(_ => _ == command.Password)))
            .ReturnsAsync(IdentityResult.Failed());

        var expected = async () => await _handler.Handle(command,CancellationToken.None);

        await expected.Should().ThrowExactlyAsync<FailedCreateUserException>();
    }
}