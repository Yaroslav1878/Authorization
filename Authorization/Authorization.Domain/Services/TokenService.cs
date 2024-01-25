using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;

namespace Authorization.Domain.Services;

public class TokenService(
        IRefreshTokenRepository refreshTokenRepository,
        ITokensConfiguration tokensConfiguration,
        ITimeProviderService timeProviderService,
        IJwtService jwtService,
        IMapper mapper,
        IUnitOfWork unitOfWork) : ITokenService
{
    private const string TokenType = "bearer";

    public AccessTokenDto IssueAccessToken(UserDto user)
    {
        if (!user.Active)
        {
            throw new UserNotFoundException(user.Email);
        }

        var jwt = jwtService.CreateJwt(user);
        var jwtString = jwtService.GetJwtString(jwt);
        jwtService.ValidateToken(jwtString);

        return new AccessTokenDto
        {
            Token = jwtString,
            Type = TokenType,
        };
    }

    public async Task<RefreshTokenDto> IssueRefreshToken(UserDto user, Guid clientId)
    {
        if (!user.Active)
        {
            throw new UserNotFoundException(user.Email);
        }

        await refreshTokenRepository.RevokeRefreshTokens(user.Id, clientId, RefreshTokenRevokeReason.Refresh);
        var now = timeProviderService.UtcNow;
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            ClientId = clientId,
            CreatedAt = now,
            ExpireAt = now.AddHours(tokensConfiguration.RefreshTokenExpirationTimeInHours),
            Token = Guid.NewGuid().ToString(),
        };

        var result = await refreshTokenRepository.InsertAsync(refreshToken);
        await unitOfWork.Commit();

        return mapper.Map<RefreshTokenDto>(result.Entity);
    }

    public async Task<RefreshTokenDto> IssueRefreshToken(RefreshTokenDto refreshToken)
    {
        if (!refreshToken.User.Active)
        {
            throw new UserNotFoundException(refreshToken.User.Email);
        }

        await refreshTokenRepository.RevokeRefreshTokens(refreshToken.UserId, refreshToken.ClientId, RefreshTokenRevokeReason.Refresh);

        var now = timeProviderService.UtcNow;
        var newRefreshToken = new RefreshToken
        {
            UserId = refreshToken.UserId,
            ClientId = refreshToken.ClientId,
            CreatedAt = now,
            ExpireAt = now.AddHours(tokensConfiguration.RefreshTokenExpirationTimeInHours),
            Token = Guid.NewGuid().ToString(),
        };

        var result = await refreshTokenRepository.InsertAsync(newRefreshToken);
        await unitOfWork.Commit();

        return mapper.Map<RefreshTokenDto>(result.Entity);
    }

    public async Task<RefreshTokenDto?> GetRefreshToken(string token)
    {
        var refreshTokenModel = await refreshTokenRepository.GetRefreshToken(token);
        return mapper.Map<RefreshTokenDto>(refreshTokenModel);
    }

    public Task LogoutRevokeRefreshTokens(RefreshTokenDto refreshToken)
    {
        return refreshTokenRepository.RevokeRefreshTokens(
            refreshToken.UserId,
            refreshToken.ClientId,
            RefreshTokenRevokeReason.Logout);
    }

    public Task LogoutRevokeRefreshTokensByUserIds(IReadOnlyCollection<int> userIds)
    {
        return refreshTokenRepository.RevokeRefreshTokensByUserIds(
            userIds.ToHashSet(),
            RefreshTokenRevokeReason.Logout);
    }
}
