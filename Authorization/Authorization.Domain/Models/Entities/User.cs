using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string RoleId { get; set; }
    public UserStatus Status { get; set; }
    [NotMapped]
    public bool Active => Status == UserStatus.Active;
    public Guid? AvatarId { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? RemovedAt { get; set; }
    public Role Role { get; set; }
    public ICollection<ConfirmationRequest> ConfirmationRequests { get; set; }
    public ICollection<UserAuth> UserAuths { get; set; }
}
