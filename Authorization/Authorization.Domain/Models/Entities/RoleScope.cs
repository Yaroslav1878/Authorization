namespace Authorization.Domain.Models.Entities;

public class RoleScope
{
    public string RoleId { get; set; }
    public string ScopeId { get; set; }
    public Role Role { get; set; }
    public Scope Scope { get; set; }
}
