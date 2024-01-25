using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.User;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Constants;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class DeleteUserHandler(
    IUserService userService,
    IMapper mapper) : IRequestHandler<DeleteUserCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // admin is changed
        var assigneeIsAdmin = userService.HasRole(request.UserId, Roles.Admin);
        if (assigneeIsAdmin)
        {
            throw new PermissionException();
        }

        UserDto userDto = await userService.Delete(request.UserId);
        return mapper.Map<UserResponseModel>(userDto);
    }
}
