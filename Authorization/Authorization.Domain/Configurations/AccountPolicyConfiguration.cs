using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations;

public class AccountPolicyConfiguration : IAccountPolicyConfiguration
{
    public int UserInvitationTimeout { get; set; }
    public int UserRegistrationTimeout { get; set; }
    public int PasswordRecoveryTimeout { get; set; }
}
