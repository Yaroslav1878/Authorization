using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Confirmation;

public class ValidateConfirmationHandler
    (IConfirmationRequestService confirmationRequestService) : IRequestHandler<ValidateConfirmationCommand, Unit>
{
    public Task<Unit> Handle(ValidateConfirmationCommand request, CancellationToken cancellationToken)
    {
        confirmationRequestService.ValidateConfirmationRequest(request.ConfirmationId, request.Hash);
        return Task.FromResult(Unit.Value);
    }
}
