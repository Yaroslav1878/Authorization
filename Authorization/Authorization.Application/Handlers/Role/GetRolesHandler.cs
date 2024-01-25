using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Role;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Role;

public class GetRolesHandler(IRoleService roleService) : IRequestHandler<GetRolesCommand, IReadOnlyCollection<string>>
{
    public async Task<IReadOnlyCollection<string>> Handle(GetRolesCommand request, CancellationToken cancellationToken)
    {
        return await roleService.Get();
    }
}
