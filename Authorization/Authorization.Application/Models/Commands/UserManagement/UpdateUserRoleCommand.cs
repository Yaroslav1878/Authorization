using Authorization.Application.Models.Responses.User;
using MediatR;

namespace Authorization.Application.Models.Commands.UserManagement;

public class UpdateUserRoleCommand : IRequest<UserResponseModel>
{
    public int UserId { get; set; }

    public string RoleId { get; set; }
}
