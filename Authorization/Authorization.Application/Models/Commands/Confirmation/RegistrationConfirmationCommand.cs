using System;
using MediatR;

namespace Authorization.Application.Models.Commands.Confirmation;

public class RegistrationConfirmationCommand : IRequest<Unit>
{
    public Guid ConfirmationId { get; set; }
}