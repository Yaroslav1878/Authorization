using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Registration;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.Registration;

public class InviteUserHandler(
        IUserService userService,
        IRegistrationService registrationService,
        IMapper mapper) : IRequestHandler<InviteUserCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        UserDto userDto = await userService.Create(request.Email, request.FirstName, request.LastName);
        await registrationService.InviteUser(userDto.Id, userDto.Email, userDto.FirstName);

        return mapper.Map<UserResponseModel>(userDto);
    }
}