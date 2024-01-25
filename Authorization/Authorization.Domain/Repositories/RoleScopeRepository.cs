using Authorization.Domain.Contexts;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;

namespace Authorization.Domain.Repositories;

public class RoleScopeRepository : GenericRepository<RoleScope>, IRoleScopeRepository
{
    public RoleScopeRepository(AuthContext context)
        : base(context.RoleScopes)
    {
    }
}
