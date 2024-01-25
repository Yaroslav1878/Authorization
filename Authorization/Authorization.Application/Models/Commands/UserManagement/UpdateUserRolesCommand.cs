using System.Collections.Generic;
using MediatR;

namespace Authorization.Application.Models.Commands.UserManagement;

public class UpdateUserRolesCommand : IRequest<Unit>
{
    public int UserId { get; set; }

    public IReadOnlyCollection<string> RoleIds { get; set; }
}
