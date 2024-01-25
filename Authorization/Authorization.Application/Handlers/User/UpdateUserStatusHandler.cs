using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.User;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Constants;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class UpdateUserStatusHandler(
    IUserService userService,
    IMapper mapper) : IRequestHandler<UpdateUserStatusCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.Status == UserStatus.Invited)
        {
            throw new BadRequestException(ErrorCode.BadRequest, "Can't update user to status 'Invited'");
        }

        // admin is changed
        var assigneeIsAdmin = userService.HasRole(request.UserId, Roles.Admin);
        if (assigneeIsAdmin)
        {
            throw new PermissionException();
        }

        UserDto userDto = await userService.UpdateStatus(request.UserId, request.Status);
        return mapper.Map<UserResponseModel>(userDto);
    }
}
