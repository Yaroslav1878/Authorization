using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Constants;
using Authorization.Application.Models.Commands.User;
using Authorization.Application.Models.Responses.User;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Constants;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class UpdateUserHandler(
        IUserService userService,
        IClaimsPrincipalService claimsPrincipalService,
        IMapper mapper) : IRequestHandler<UpdateUserCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // admin is changed
        var assigneeIsAdmin = userService.HasRole(request.UserId, Roles.Admin);
        if (assigneeIsAdmin && !claimsPrincipalService.HasScope(Scopes.ManageUsers))
        {
            throw new PermissionException();
        }

        var userDto = await userService.UpdateUser(request.UserId, request.FirstName, request.LastName);
        return mapper.Map<UserResponseModel>(userDto);
    }
}
