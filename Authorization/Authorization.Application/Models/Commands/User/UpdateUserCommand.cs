using Authorization.Application.Models.Responses.User;
using MediatR;

namespace Authorization.Application.Models.Commands.User;

public class UpdateUserCommand : IRequest<UserResponseModel>
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
