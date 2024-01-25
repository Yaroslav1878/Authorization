using Authorization.Application.Models.Responses.User;
using MediatR;

namespace Authorization.Application.Models.Commands.User;

public class GetUserCommand : IRequest<UserResponseModel>
{
    public int UserId { get; set; }
}
