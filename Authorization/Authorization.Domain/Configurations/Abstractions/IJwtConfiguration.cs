namespace Authorization.Domain.Configurations.Abstractions;

public interface IJwtConfiguration
{
    public string Issuer { get; }
    public string Authority { get; }
    public int ExpirationTimeInMinutes { get; }
}
