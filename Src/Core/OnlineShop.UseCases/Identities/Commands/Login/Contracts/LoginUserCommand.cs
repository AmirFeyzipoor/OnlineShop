using MediatR;

namespace OnlineShop.UseCases.Identities.Commands.Login.Contracts;

public class LoginUserCommand : IRequest<string>
{
    public LoginUserCommand(string password, string userName)
    {
        Password = password;
        UserName = userName;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
}