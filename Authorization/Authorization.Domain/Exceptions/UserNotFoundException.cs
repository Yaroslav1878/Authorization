using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class UserNotFoundException : ApplicationException
{
    private const string UserIdNotFound = "Not found user by UserId: ";
    private const string UserEmailNotFound = "Not found user by email: ";

    public UserNotFoundException()
        : base(ErrorCode.UserNotFoundException, HttpStatusCode.NotFound, "User not found")
    {
    }

    public UserNotFoundException(int userId)
        : base(ErrorCode.UserIdNotFoundException, HttpStatusCode.NotFound, UserIdNotFound + userId)
    {
    }

    public UserNotFoundException(string email)
        : base(ErrorCode.UserEmailNotFoundException, HttpStatusCode.NotFound, UserEmailNotFound + email)
    {
    }
}
