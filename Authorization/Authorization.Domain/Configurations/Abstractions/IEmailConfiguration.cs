namespace Authorization.Domain.Configurations.Abstractions
{
    public interface IEmailConfiguration
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
