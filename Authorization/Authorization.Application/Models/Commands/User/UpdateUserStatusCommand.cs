using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Enums;
using MediatR;

namespace Authorization.Application.Models.Commands.User;

public class UpdateUserStatusCommand : IRequest<UserResponseModel>
{
    public int UserId { get; set; }
    public UserStatus Status { get; set; }
}
