using System;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Models.Entities;

public class UserAuth
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public AuthType AuthType { get; set; }

    public Subject Subject { get; set; }

    public string? PasswordHash { get; set; }

    public string? Salt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; }
}
