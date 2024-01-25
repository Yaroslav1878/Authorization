using System;
using System.Collections.Generic;
using Authorization.Domain.Enums;

namespace Authorization.Application.Models.Responses.User;

public class UserResponseModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; }
    public UserStatus Status { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? AvatarId { get; set; }
    public string Role { get; set; }
    public IReadOnlyCollection<string> Scopes { get; set; }
}
