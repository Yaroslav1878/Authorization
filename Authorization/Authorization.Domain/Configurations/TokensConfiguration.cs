using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations
{
    public class TokensConfiguration : ITokensConfiguration
    {
        public bool ShouldIssueRefreshTokens { get; set; }
        public int RefreshTokenExpirationTimeInHours { get; set; }
        public IJwtConfiguration JwtConfiguration { get; set; } = new JwtConfiguration();
    }
}
