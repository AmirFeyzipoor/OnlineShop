using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts.Exceptions;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts.TokenConfigs;

namespace OnlineShop.UseCases.Identities.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;

    public LoginUserCommandHandler(
        UserManager<User> userManager,
        IOptions<JwtBearerTokenSettings> jwtBearerTokenSettings)
    {
        _userManager = userManager;
        _jwtBearerTokenSettings = jwtBearerTokenSettings.Value;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        
        StopIfUserNotFound(user);

        await StopIfWrongUserNameOrPassword(request.Password, user!);
        
        var userClaims = await _userManager.GetClaimsAsync(user!);
        
        return GenerateToken(user!.Id, userClaims);
    }

    private static void StopIfUserNotFound(User? user)
    {
        if (user == null)
            throw new UserNotFoundException();
    }

    private string GenerateToken(string userId, IList<Claim> userClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

        var tokenClaims = new ClaimsIdentity();
        tokenClaims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

        WriteUserClaimsToTokenClaims(ref tokenClaims, userClaims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = tokenClaims,

            Expires = DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtBearerTokenSettings.Audience,
            Issuer = _jwtBearerTokenSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private void WriteUserClaimsToTokenClaims(
        ref ClaimsIdentity tokenClaims, 
        IList<Claim> userClaims)
    {
        foreach (var claim in userClaims)
        {
            tokenClaims.AddClaim(new Claim(claim.Type, claim.Value));
        }
    }

    private async Task StopIfWrongUserNameOrPassword(string password, User user)
    {
        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, password);
        if (isCorrectPassword == false)
            throw new WrongUserNameOrPasswordException();
    }
}