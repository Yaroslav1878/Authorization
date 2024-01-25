using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Entities;

namespace Authorization.Domain.Repositories.Abstractions;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task RevokeRefreshTokens(int userId, Guid clientId, RefreshTokenRevokeReason reason);

    Task RevokeRefreshTokensByUserIds(HashSet<int> userIds, RefreshTokenRevokeReason reason);

    Task<RefreshToken?> GetRefreshToken(string refreshToken);
}
