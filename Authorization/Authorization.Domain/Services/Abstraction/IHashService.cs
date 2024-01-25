namespace Authorization.Domain.Services.Abstraction;

public interface IHashService
{
    string CreateSalt();
    string CreateHash(string valueToHash, string salt);
    bool VerifyPassword(string password, string salt, string passwordHash);
}
