using System;
using MediatR;

namespace Authorization.Application.Models.Commands.User;

public class UpdateUserPasswordCommand : IRequest<Unit>
{
    public Guid ConfirmationId { get; set; }
    public string NewPassword { get; set; }
}
