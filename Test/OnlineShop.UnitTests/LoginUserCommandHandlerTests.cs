using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Login;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts.Exceptions;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts.TokenConfigs;

namespace OnlineShop.UnitTests;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
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
        var options = Options.Create(new JwtBearerTokenSettings()
        {
            Audience = "OnlineShop",
            Issuer = "OnlineShopIdentity",
            SecretKey = "OnlineShopIdentityEncryptionKey",
            ExpiryTimeInSeconds = 360000
        });
        _handler = new LoginUserCommandHandler(_mockUserManager.Object, options);
    }

    [Fact]
    public async Task Handle_login_user_proper()
    {
        var command = new LoginUserCommand(userName: "Amir", password: "Amir007");
        var user = new User()
        {
            Id = "86ad0589-03c1-4b36-8159-a6219f5dde60",
            CreationDate = DateTime.Now.ToUniversalTime(),
            UserName = command.UserName,
            PasswordHash = "AQAAAAIAAYagAAAAENdGlm54FHutArTsa9iwkaDCCuije8I1UnkFFDP1WvDRzvz/cWjbrLqArEnunG/pJg=="
        };
        
        _mockUserManager.Setup(_ => _.FindByNameAsync(
                It.Is<string>(_ => _ == command.UserName)))
            .ReturnsAsync(user);
        
        _mockUserManager.Setup(_ => _.CheckPasswordAsync(
                It.Is<User>(_ => _ == user),
                It.Is<string>(_ => _ == command.Password)))
            .ReturnsAsync(true);
        
        _mockUserManager.Setup(_ => _.GetClaimsAsync(
                It.Is<User>(_ => _ == user)))
            .ReturnsAsync(new List<Claim>());

        var token = await _handler.Handle(command, CancellationToken.None);

        token.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Handle_throw_exception_when_user_not_found()
    {
        var command = new LoginUserCommand(userName: "Amir", password: "Amir007");

        _mockUserManager.Setup(_ => _.FindByNameAsync(
                It.Is<string>(_ => _ == command.UserName)))
            .ReturnsAsync((User?)null);

        var expected = async () => await _handler.Handle(command, CancellationToken.None);

        await expected.Should().ThrowExactlyAsync<UserNotFoundException>();
    }
    
    [Fact]
    public async Task Handle_throw_exception_when_wrong_username_or_password()
    {
        var command = new LoginUserCommand(userName: "Amir", password: "Amir007");
        var user = new User()
        {
            Id = "86ad0589-03c1-4b36-8159-a6219f5dde60",
            CreationDate = DateTime.Now.ToUniversalTime(),
            UserName = command.UserName,
            PasswordHash = "wrong-password"
        };
        
        _mockUserManager.Setup(_ => _.FindByNameAsync(
                It.Is<string>(_ => _ == command.UserName)))
            .ReturnsAsync(user);
        
        _mockUserManager.Setup(_ => _.CheckPasswordAsync(
                It.Is<User>(_ => _ == user),
                It.Is<string>(_ => _ == command.Password)))
            .ReturnsAsync(false);

        var expected = async () => await _handler.Handle(command, CancellationToken.None);

        await expected.Should().ThrowExactlyAsync<WrongUserNameOrPasswordException>();
    }
}