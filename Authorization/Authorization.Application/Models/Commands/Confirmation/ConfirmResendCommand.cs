using MediatR;

namespace Authorization.Application.Models.Commands.Confirmation;

public class ConfirmResendCommand : IRequest<Unit>
{
    public int UserId { get; set; }
}
