using System;
using System.Collections.Generic;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Entities;

namespace Authorization.Domain.Models.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; }
    public UserStatus Status { get; set; }
    public bool Active => Status == UserStatus.Active;
    public Guid? AvatarId { get; set; }
    public string RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public RoleDto Role { get; set; }

    public ICollection<UserAuth> UserAuths { get; set; }
}
