using System;
using MediatR;

namespace Authorization.Application.Models.Commands.Confirmation
{
    public class ValidateConfirmationCommand : IRequest<Unit>
    {
        public Guid ConfirmationId { get; set; }
        public string Hash { get; set; }
    }
}
