using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Constants;
using Authorization.Application.Models.Commands.UserManagement;
using Authorization.Application.Models.Responses.User;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Constants;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class UpdateUserRolesHandler(
    IUserService userService,
    IMapper mapper,
    IClaimsPrincipalService claimsPrincipalService) : IRequestHandler<UpdateUserRoleCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var isAdmin = userService.HasRole(request.UserId, Roles.Admin);
        if (!isAdmin || !claimsPrincipalService.HasScope(Scopes.ManageUsers))
        {
            throw new PermissionException();
        }

        var response = await userService.UpdateUserRole(request.UserId, request.RoleId);
        return mapper.Map<UserResponseModel>(response);
    }
}