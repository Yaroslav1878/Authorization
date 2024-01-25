using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations;

public class JwtConfiguration : IJwtConfiguration
{
    public string Issuer { get; set; }

    public string Authority { get; set; }

    public int ExpirationTimeInMinutes { get; set; }

    public string SymmetricKey { get; set; }
}