using System;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;

namespace Authorization.Domain.Services;

public class RegistrationService(
        IAccountPolicyConfiguration accountPolicyConfiguration,
        ITimeProviderService timeProviderService,
        IConfirmationRequestService confirmationRequestService,
        IUnitOfWork unitOfWork) : IRegistrationService
{
    public async Task InviteUser(int userId, string email, string firstName)
    {
        await confirmationRequestService.Revoke(userId, ConfirmationRequestSubject.Invite);

        var confirmationRequest = new ConfirmationRequestDto
        {
            Id = Guid.NewGuid(),
            CreatedAt = timeProviderService.UtcNow,
            ExpiredAt = timeProviderService.UtcNow.AddMonths(accountPolicyConfiguration.UserInvitationTimeout),
            UserId = userId,
            Subject = ConfirmationRequestSubject.Invite,
            ConfirmationType = ConfirmationRequestType.Email,
            Receiver = email,
        };
        await confirmationRequestService.Create(confirmationRequest);

        await unitOfWork.Commit();
    }
    
    public async Task RegisterUser(int userId, string email, string firstName)
    {
        await confirmationRequestService.Revoke(userId, ConfirmationRequestSubject.Registration);

        var confirmationRequest = new ConfirmationRequestDto
        {
            Id = Guid.NewGuid(),
            CreatedAt = timeProviderService.UtcNow,
            ExpiredAt = timeProviderService.UtcNow.AddMonths(accountPolicyConfiguration.UserRegistrationTimeout),
            UserId = userId,
            Subject = ConfirmationRequestSubject.Registration,
            ConfirmationType = ConfirmationRequestType.Email,
            Receiver = email,
        };
        await confirmationRequestService.Create(confirmationRequest);

        await unitOfWork.Commit();
    }
}
