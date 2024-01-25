using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Registration;
using Authorization.Application.Models.Responses.User;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Enums;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.Registration;

public class RegisterUserHandler(
        IUserService userService,
        IRegistrationService registrationService,
        IMapper mapper) : IRequestHandler<RegisterUserCommand, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userDto = await userService.Create(request.Email, request.FirstName, request.LastName);

        await userService.CreateUserAuth(userDto.Id, AuthType.Email, request.Password);
        await registrationService.RegisterUser(userDto.Id, userDto.Email, userDto.FirstName);

        return mapper.Map<UserResponseModel>(userDto);
    }
}