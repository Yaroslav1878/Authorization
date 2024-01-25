using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SendGrid.Helpers.Mail;

namespace Authorization.Domain.Services
{
    public class ConfirmationRequestService(
            IConfirmationRequestRepository confirmationRequestRepository,
            ITimeProviderService timeProviderService,
            IHashService hashService,
            IEmailConfiguration emailConfiguration,
          //  ISendGridConfiguration sendGridConfiguration,
          //  IEmailService emailService,
            IUnitOfWork unitOfWork,
            IMapper mapper) : IConfirmationRequestService
    {
        private const string RequestIdParam = "id";
        private const string HashParam = "hash";

        public async Task<ConfirmationRequestDto> Create(ConfirmationRequestDto confirmationRequest)
        {
            EntityEntry<ConfirmationRequest> result = await confirmationRequestRepository.InsertAsync(
                mapper.Map<ConfirmationRequest>(confirmationRequest));
            await unitOfWork.Commit();

            var nameValueCollection = GetUrlQuery(result.Entity.Id);
            var uriEmailConfirmationLink = new Uri(emailConfiguration.EmailConfirmationFilePath + nameValueCollection);
            
            //todo: setup SendGrid
            /*var emailData = new EmailData
            {
                UriLink = uriEmailConfirmationLink,
                Sender = new EmailAddress(sendGridConfiguration.SenderEmail, sendGridConfiguration.SenderName),
                Recipients = new List<EmailAddress> { new (confirmationRequest.User.Email, confirmationRequest.User.FirstName) },
                EmailTemplate = EmailTemplate.ConfirmationEmail,
                AdditionalSubject = AdditionalSubject.Registration,
            };

            await emailService.SendEmail(emailData);*/

            return mapper.Map<ConfirmationRequestDto>(result.Entity);
        }

        public ConfirmationRequestDto? Get(Guid confirmationId)
        {
            var confirmationRequest = confirmationRequestRepository
                .FirstOrDefault(confirmationRequest => confirmationRequest.Id == confirmationId,
                    confirmationRequest => confirmationRequest.User);

            return mapper.Map<ConfirmationRequestDto>(confirmationRequest);
        }

        public async Task Revoke(int userId, ConfirmationRequestSubject subject)
        {
            var notRevokedRequests = await confirmationRequestRepository.Find(confirmationRequest =>
                confirmationRequest.UserId == userId
                && confirmationRequest.Subject == subject
                && !confirmationRequest.ConfirmedAt.HasValue);

            notRevokedRequests.ForEach(confirmationRequest =>
            {
                confirmationRequest.RevokedAt = timeProviderService.UtcNow;
            });
            await unitOfWork.Commit();
        }
        
        public async Task Confirm(int userId, ConfirmationRequestSubject subject)
        {
            var notRevokedRequests = await confirmationRequestRepository.Find(confirmationRequest =>
                confirmationRequest.UserId == userId
                && confirmationRequest.Subject == subject
                && !confirmationRequest.ConfirmedAt.HasValue);

            notRevokedRequests.ForEach(confirmationRequest =>
            {
                confirmationRequest.ConfirmedAt = timeProviderService.UtcNow;
            });
            await unitOfWork.Commit();
        }

        public NameValueCollection GetUrlQuery(Guid confirmationRequestId)
        {
            NameValueCollection nameValueCollection = System.Web.HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("?" + RequestIdParam, confirmationRequestId.ToString());
            string confirmationHash = GetHash(confirmationRequestId);
            nameValueCollection.Add(HashParam, confirmationHash);

            return nameValueCollection;
        }

        public string GetHash(Guid confirmationId)
        {
            return hashService.CreateHash(confirmationId.ToString(), string.Empty);
        }
        
        public void ValidateConfirmationRequest(Guid confirmationId, string hash)
        {
            var confirmationRequest = Get(confirmationId);

            if (confirmationRequest == null)
            {
                throw new ConfirmationRequestNotFoundException();
            }

            string dbConfirmationRequestHash = GetHash(confirmationId);

            if (!hash.Equals(dbConfirmationRequestHash, StringComparison.InvariantCulture))
            {
                throw new ConfirmationRequestNotFoundException();
            }

            if (confirmationRequest.ExpiredAt < timeProviderService.UtcNow)
            {
                throw new ConfirmationRequestExpiredException();
            }

            if (confirmationRequest.ConfirmedAt.HasValue)
            {
                throw new ConfirmationWasUsedException();
            }
        }
    }
}
