using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts.Events;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts.Exceptions;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Identities.Commands.Add;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        UserManager<User> userManager,
        IMediator mediator,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        await StopIfDuplicatedUserName(command.UserName);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = command.UserName,
            CreationDate = DateTime.Now.ToUniversalTime()
        };

        var result = await _userManager.CreateAsync(user, command.Password);

        StopIfCreateUserFailed(result);

        var updateReadableDbEvent = new UpdateReadableDbAfterRegisterUserEvent(
            user.Id, user.UserName, user.CreationDate);
        await _mediator.Publish(updateReadableDbEvent, CancellationToken.None);

        await _unitOfWork.SaveChangesAsync();

        return user.Id;
    }

    private static void StopIfCreateUserFailed(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new FailedCreateUserException();
    }

    private async Task StopIfDuplicatedUserName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user != null)
            throw new DuplicatedUserNameException();
    }
}