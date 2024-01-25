using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Token;
using Authorization.Application.Models.Responses.Token;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.Token;

public class RefreshAccessTokenHandler(
        ITokenService tokenService,
        ITimeProviderService timeProviderService,
        IMapper mapper,
        ITokensConfiguration tokensConfiguration) : IRequestHandler<RefreshTokenCommand, TokenResponseModel>
{
    public async Task<TokenResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        RefreshTokenDto? refreshToken = await tokenService.GetRefreshToken(request.RefreshToken);

        var refreshTokenNotValid = refreshToken == null
                                   || refreshToken.ExpireAt < timeProviderService.UtcNow
                                   || refreshToken.RevokeReason == RefreshTokenRevokeReason.Logout;

        if (refreshTokenNotValid)
        {
            throw new AuthorizationRefreshException();
        }

        AccessTokenDto accessToken = tokenService.IssueAccessToken(refreshToken!.User);

        refreshToken = await tokenService.IssueRefreshToken(refreshToken);

        if (refreshToken.ExpireAt < timeProviderService.UtcNow)
        {
            throw new AuthorizationRefreshException();
        }

        UserResponseModel user = mapper.Map<UserResponseModel>(refreshToken!.User);

        var response = new TokenResponseModel(accessToken, refreshToken, user, tokensConfiguration);
        return response;
    }
}
