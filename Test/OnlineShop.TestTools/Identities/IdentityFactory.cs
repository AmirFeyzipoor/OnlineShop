using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Add;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.TestTools.Identities;

public static class IdentityFactory
{
    public static RegisterUserCommandHandler GenerateRegisterUserCommandHandler(
        Mock<UserManager<User>> mockUserManager,
        Mock<IMediator> mockMediator,
        Mock<IUnitOfWork> mockUnitOfWork)
    {
        return new RegisterUserCommandHandler(
            mockUserManager.Object,
            mockMediator.Object,
            mockUnitOfWork.Object);
    }
}