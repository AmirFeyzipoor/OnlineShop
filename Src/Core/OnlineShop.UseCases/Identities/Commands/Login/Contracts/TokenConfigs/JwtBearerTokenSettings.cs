namespace OnlineShop.UseCases.Identities.Commands.Login.Contracts.TokenConfigs;

public class JwtBearerTokenSettings
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecretKey { get; set; }
    public double ExpiryTimeInSeconds { get; set; }
}