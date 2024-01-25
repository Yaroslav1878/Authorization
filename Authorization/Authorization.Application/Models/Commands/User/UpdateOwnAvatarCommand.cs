using System;
using Authorization.Application.Models.Responses.User;
using MediatR;

namespace Authorization.Application.Models.Commands.User;

public class UpdateOwnAvatarCommand : IRequest<UserResponseModel>
{
    public int UserId { get; set; }

    public Guid AvatarId { get; set; }
}