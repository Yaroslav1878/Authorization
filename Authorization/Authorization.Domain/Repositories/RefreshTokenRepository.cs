using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Domain.Contexts;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Domain.Repositories;
    public class RefreshTokenRepository(AuthContext context) : GenericRepository<RefreshToken>(context.RefreshTokens),
        IRefreshTokenRepository
{
    public async Task RevokeRefreshTokens(int userId, Guid clientId, RefreshTokenRevokeReason reason)
    {
        var now = DateTime.UtcNow;
        var existingRefreshTokens = await Find(refreshToken => refreshToken.UserId == userId &&
                                                               refreshToken.ClientId == clientId &&
                                                               refreshToken.ExpireAt > now &&
                                                               refreshToken.RevokedAt == null);

        foreach (var existingRefreshToken in existingRefreshTokens)
        {
            existingRefreshToken.RevokedAt = now;
            existingRefreshToken.RevokeReason = reason;
        }
    }

    public async Task RevokeRefreshTokensByUserIds(HashSet<int> userIds, RefreshTokenRevokeReason reason)
    {
        var now = DateTime.UtcNow;
        List<RefreshToken> existingRefreshTokens = await Find(refreshToken => userIds.Contains(refreshToken.UserId) &&
                                                                              refreshToken.ExpireAt > now &&
                                                                              refreshToken.RevokedAt == null);

        foreach (var existingRefreshToken in existingRefreshTokens)
        {
            existingRefreshToken.RevokedAt = now;
            existingRefreshToken.RevokeReason = reason;
        }

        context.RefreshTokens.UpdateRange(existingRefreshTokens);
    }

    public async Task<RefreshToken?> GetRefreshToken(string refreshTokenKey)
    {
        return await context.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .ThenInclude(user => user.Role)
            .ThenInclude(role => role.RoleScopes)
            .Where(refreshToken => refreshToken.Token == refreshTokenKey)
            .FirstOrDefaultAsync();
    }
}
