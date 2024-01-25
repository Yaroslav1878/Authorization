using System;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Models.Dtos;

public class RefreshTokenDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public RefreshTokenRevokeReason? RevokeReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public ClientDto Client { get; set; }
    public UserDto User { get; set; }
}
