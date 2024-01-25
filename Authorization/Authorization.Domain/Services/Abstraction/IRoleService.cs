using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Domain.Services.Abstraction;

public interface IRoleService
{
    Task<IReadOnlyCollection<string>> Get();

    Task ValidateRolesIds(IReadOnlyCollection<string> roleIds);
}
