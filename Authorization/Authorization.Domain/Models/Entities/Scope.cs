using System.Collections.Generic;

namespace Authorization.Domain.Models.Entities;

public class Scope
{
    public string Id { get; set; }
    public ICollection<RoleScope> RoleScopes { get; set; }
}
