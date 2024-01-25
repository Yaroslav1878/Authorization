using MediatR;

namespace Authorization.Application.Models.Commands.Confirmation;

public class PasswordRecoveryCommand : IRequest<Unit>
{
    public string Email { get; set; }
}
