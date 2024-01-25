namespace Authorization.Domain.Configurations.Abstractions;

public interface IAccountPolicyConfiguration
{
    int UserInvitationTimeout { get; set; }
    
    int UserRegistrationTimeout { get; set; }

    int PasswordRecoveryTimeout { get; set; }
}