using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Models.Dtos;

namespace Authorization.Application.Models.Responses.Token;

public class TokenResponseModel(
    AccessTokenDto accessToken,
    RefreshTokenDto refreshToken,
    UserResponseModel user,
    ITokensConfiguration tokensConfiguration)
{
    public AccessTokenResponseModel AccessTokenResponseModel { get; set; } = new()
    {
        AccessToken = accessToken.Token,
        TokenType = accessToken.Type,
        UserDetails = user,
        CreatedAt = refreshToken.CreatedAt,
        RefreshTokenExpiredAt = refreshToken.ExpireAt,
        AccessTokenExpiredAt =
            refreshToken.CreatedAt.AddMinutes(tokensConfiguration.JwtConfiguration.ExpirationTimeInMinutes)
    };

    public RefreshTokenResponseModel RefreshTokenResponseModel { get; set; } = new()
    {
        RefreshToken = refreshToken.Token,
    };
}
