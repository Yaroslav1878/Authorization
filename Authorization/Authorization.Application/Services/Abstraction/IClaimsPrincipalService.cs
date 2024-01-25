namespace Authorization.Application.Services.Abstraction;

public interface IClaimsPrincipalService
{
    int GetAuthorizedUserId();
    bool HasScope(string scope);
}
