using System.Collections.Generic;

namespace Authorization.Domain.Models.Dtos;

public class ScopeDto
{
    public string Id { get; set; }
    public IReadOnlyCollection<RoleScopeDto> RoleScopes { get; set; }
}
