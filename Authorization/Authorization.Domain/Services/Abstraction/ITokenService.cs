using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Domain.Models.Dtos;

namespace Authorization.Domain.Services.Abstraction;

public interface ITokenService
{
    AccessTokenDto IssueAccessToken(UserDto user);
    Task<RefreshTokenDto> IssueRefreshToken(UserDto user, Guid client);
    Task<RefreshTokenDto> IssueRefreshToken(RefreshTokenDto refreshToken);
    Task<RefreshTokenDto?> GetRefreshToken(string refreshToken);
    Task LogoutRevokeRefreshTokens(RefreshTokenDto refreshToken);
    Task LogoutRevokeRefreshTokensByUserIds(IReadOnlyCollection<int> userIds);
}