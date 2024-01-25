using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public int ConfirmationRequestInMinutes { get; set; }
        public int PasswordRecoveryRequestInMinutes { get; set; }
        public string AuthorizationFilePath { get; set; }
        public string WebPortalFilePath { get; set; }
        public string EmailConfirmationFilePath { get; set; }
        public string PasswordRecoveryFilePath { get; set; }
        public string AcceptInvitationFilePath { get; set; }
    }
}
