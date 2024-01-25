using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Confirmation;

public class PasswordRecoveryHandler(IUserService userService) : IRequestHandler<PasswordRecoveryCommand, Unit>
{
    public async Task<Unit> Handle(PasswordRecoveryCommand request, CancellationToken cancellationToken)
    {
        await userService.SendPasswordResetEmail(request.Email);
        return Unit.Value;
    }
}
