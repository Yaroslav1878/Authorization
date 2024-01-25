using System.Collections.Generic;
using Authorization.Application.Models.Responses.User;
using MediatR;

namespace Authorization.Application.Models.Commands.UserManagement;

public class GetUsersCommand : IRequest<IReadOnlyCollection<UserResponseModel>>
{
    public IReadOnlyCollection<int> UserIds { get; set; }
    public IReadOnlyCollection<string> RolesFilter { get; set; }
}
