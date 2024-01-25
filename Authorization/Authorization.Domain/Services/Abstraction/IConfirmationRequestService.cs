using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Dtos;

namespace Authorization.Domain.Services.Abstraction;

public interface IConfirmationRequestService
{
    Task<ConfirmationRequestDto> Create(ConfirmationRequestDto confirmationRequest);
    ConfirmationRequestDto? Get(Guid confirmationId);
    Task Revoke(int userId, ConfirmationRequestSubject subject);
    Task Confirm(int userId, ConfirmationRequestSubject subject);
    NameValueCollection GetUrlQuery(Guid confirmationRequestId);
    string GetHash(Guid confirmationId);
    void ValidateConfirmationRequest(Guid confirmationId, string hash);
}
