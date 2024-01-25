using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Domain.Enums;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Confirmation;

public class RegistrationConfirmationHandler(
    IConfirmationRequestService confirmationRequestService,
    IUserService userService) : IRequestHandler<RegistrationConfirmationCommand, Unit>
{
    public async Task<Unit> Handle(RegistrationConfirmationCommand request, CancellationToken cancellationToken)
    {
        var confirmationRequest = confirmationRequestService.Get(request.ConfirmationId);

        await confirmationRequestService.Confirm(confirmationRequest!.UserId, ConfirmationRequestSubject.Registration);
        await userService.UpdateStatus(confirmationRequest.UserId, UserStatus.Active);
        return Unit.Value;
    }
}
