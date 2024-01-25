using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;

namespace Authorization.Domain.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<IReadOnlyCollection<string>> Get()
    {
        var roles = await roleRepository.FindAll();
        return roles.Select(x => x.Id).ToList();
    }

    public async Task ValidateRolesIds(IReadOnlyCollection<string> roleIds)
    {
        var existingRolesArray = await roleRepository.FindAll();
        var inputRoleId = roleIds.FirstOrDefault(inputRole =>
            existingRolesArray.All(existingRoles => !existingRoles.Id.Equals(inputRole)));

        if (inputRoleId != null)
        {
            throw new RoleNotFoundException(inputRoleId);
        }
    }
}
