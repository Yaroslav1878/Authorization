using System.Collections.Generic;
using MediatR;

namespace Authorization.Application.Models.Commands.Role;

public class GetAssignableRolesCommand : IRequest<IReadOnlyCollection<string>>
{
    public int? UserId { get; set; }
}
