namespace Authorization.Domain.Configurations.Abstractions
{
    public interface ITokensConfiguration
    {
        public bool ShouldIssueRefreshTokens { get; }
        public int RefreshTokenExpirationTimeInHours { get; }
        public IJwtConfiguration JwtConfiguration { get; }
    }
}
