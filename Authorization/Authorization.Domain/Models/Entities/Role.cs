using System.Collections.Generic;

namespace Authorization.Domain.Models.Entities;

public class Role
{
    public string Id { get; set; }
    public ICollection<RoleScope> RoleScopes { get; set; }
    public ICollection<User> Users { get; set; }
}
