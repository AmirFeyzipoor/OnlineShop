using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineShop.UseCases.Identities.Commands.Add.Contracts;

public class RegisterUserCommand : IRequest<string>
{
    public RegisterUserCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}