using System;
using MediatR;

namespace Authorization.Application.Models.Commands.Confirmation;

public class InviteConfirmationCommand : IRequest<Unit>
{
    public Guid ConfirmationId { get; set; }

    public string Password { get; set; }
}