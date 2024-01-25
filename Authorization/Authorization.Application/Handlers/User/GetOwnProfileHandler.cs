using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.User;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class GetUserHandler(
    IUserService userService,
    IMapper mapper) : IRequestHandler<GetUserCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        UserDto user = await userService.GetById(request.UserId);

        if (user.Status == UserStatus.Deleted)
        {
            throw new UserNotFoundException(request.UserId);
        }

        return mapper.Map<UserResponseModel>(user);
    }
}
