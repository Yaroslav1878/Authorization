using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Constants;
using Authorization.Application.Models.Commands.Role;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Constants;
using MediatR;

namespace Authorization.Application.Handlers.Role;

public class GetAssignableRolesHandler(IClaimsPrincipalService claimsPrincipalService)
    : IRequestHandler<GetAssignableRolesCommand, IReadOnlyCollection<string>>
{
    private readonly List<string> _adminAssignableRoles = new()
    {
        Roles.UnconfirmedUser,
        Roles.User,
    };

    public Task<IReadOnlyCollection<string>> Handle(
        GetAssignableRolesCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<string>>(GetInvitableUserRoles());
    }

    private List<string> GetInvitableUserRoles()
    {
        if (claimsPrincipalService.HasScope(Scopes.ManageUsers))
        {
            return _adminAssignableRoles;
        }

        return new List<string>();
    }
}