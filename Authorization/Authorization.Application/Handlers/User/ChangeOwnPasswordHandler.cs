using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.User;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class UpdateOwnPasswordHandler(
        IConfirmationRequestService confirmationRequestService,
        IUserService userService,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserPasswordCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var confirmationRequest = confirmationRequestService.Get(request.ConfirmationId);

        if (confirmationRequest == null)
        {
            throw new EntityNotFoundException("confirmation request");
        }

        userService.UpdatePassword(confirmationRequest.UserId, request.NewPassword);
        await confirmationRequestService.Confirm(confirmationRequest.UserId, ConfirmationRequestSubject.PasswordRecovery);
        await unitOfWork.Commit();
        return Unit.Value;
    }
}
