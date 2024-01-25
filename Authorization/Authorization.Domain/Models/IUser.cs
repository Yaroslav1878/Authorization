using System;
using System.Collections.Generic;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;

namespace Authorization.Domain.Models
{
    public interface IUser
    {
        public int Id { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public Guid AvatarId { get; }
        public string Salt { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string RoleId { get; }
        public RoleDto Role { get; }
        public bool Active { get; }
        public DateTime CreatedAt { get; }
        public DateTime? RemovedAt { get; }
        public ICollection<UserAuth> UserAuths { get; set; }
        public ICollection<ConfirmationRequest> ConfirmationRequests { get; }
    }
}
