using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Confirmation;

public class PasswordConfirmationHandler(IUserService userService) : IRequestHandler<PasswordConfirmationCommand, Unit>
{
    public async Task<Unit> Handle(PasswordConfirmationCommand request, CancellationToken cancellationToken)
    {
        await userService.ResetPassword(request.ConfirmationId, request.Password);
        return Unit.Value;
    }
}
