using System.Threading.Tasks;

namespace Authorization.Domain.Services.Abstraction;

public interface IRegistrationService
{
    Task InviteUser(int userId, string email, string firstName);
    Task RegisterUser(int userId, string email, string firstName);
}
