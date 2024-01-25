using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Domain.Enums;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Confirmation;

public class InviteConfirmationHandler(
        IConfirmationRequestService confirmationRequestService,
        IUserService userService) : IRequestHandler<InviteConfirmationCommand, Unit>
{
    public async Task<Unit> Handle(InviteConfirmationCommand request, CancellationToken cancellationToken)
    {
        var confirmationRequest = confirmationRequestService.Get(request.ConfirmationId);

        await confirmationRequestService.Confirm(confirmationRequest!.UserId, ConfirmationRequestSubject.Invite);
        await userService.CreateUserAuth(confirmationRequest.UserId, AuthType.Email, request.Password);
        await userService.UpdateStatus(confirmationRequest.UserId, UserStatus.Active);
        return Unit.Value;
    }
}
