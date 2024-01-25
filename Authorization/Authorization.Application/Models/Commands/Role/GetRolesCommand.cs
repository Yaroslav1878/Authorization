using System.Collections.Generic;
using MediatR;

namespace Authorization.Application.Models.Commands.Role;

public class GetRolesCommand : IRequest<IReadOnlyCollection<string>>
{
}
