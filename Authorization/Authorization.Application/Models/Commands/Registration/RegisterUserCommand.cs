using Authorization.Application.Models.Responses.User;
using MediatR;

namespace Authorization.Application.Models.Commands.Registration;

public class RegisterUserCommand : IRequest<UserResponseModel>
{
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Password { get; set; }
}