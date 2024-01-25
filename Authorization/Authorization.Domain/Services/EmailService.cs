using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models;
using Authorization.Domain.Services.Abstraction;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Authorization.Domain.Services
{
    public class EmailService(ISendGridConfiguration sendGridConfiguration) : IEmailService
    {
        public async Task SendEmail(EmailData emailData)
        {
            var sendGridClient = new SendGridClient(sendGridConfiguration.ApiKey);
            var from = new EmailAddress(emailData.Sender.Email, emailData.Sender.Name);
            var subject = emailData.AdditionalSubject.ToString();
            var recipients = emailData.Recipients.FirstOrDefault();
            var to = new EmailAddress(recipients?.Email, recipients?.Name);
            var plainContent = "Hello";
            var htmlContent = $"<h1>Please, confirm {subject} by link: {emailData.UriLink}</h1>";
            var mailMessage = MailHelper.CreateSingleEmail(from, to, subject, plainContent, htmlContent);

            var response = await sendGridClient.SendEmailAsync(mailMessage);
            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new SendGridException(response.StatusCode);
            }
        }
    }
}
