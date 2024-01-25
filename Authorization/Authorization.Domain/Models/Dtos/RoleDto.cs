using System.Collections.Generic;

namespace Authorization.Domain.Models.Dtos;

public class RoleDto
{
    public string Id { get; set; }
    public IReadOnlyCollection<RoleScopeDto> RoleScopes { get; set; }
    public IReadOnlyCollection<UserDto> Users { get; set; }
}
