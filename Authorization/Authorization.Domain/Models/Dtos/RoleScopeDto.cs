namespace Authorization.Domain.Models.Dtos;

public class RoleScopeDto
{
    public string RoleId { get; set; }
    public string ScopeId { get; set; }
    public RoleDto Role { get; set; }
    public ScopeDto Scope { get; set; }
}
