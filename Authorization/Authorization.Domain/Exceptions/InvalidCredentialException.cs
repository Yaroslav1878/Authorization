using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class InvalidCredentialException : ApplicationException
{
    private const string InvalidPassword = "Email or password is invalid.";

    public InvalidCredentialException()
        : base(ErrorCode.InvalidCredentialsException,  HttpStatusCode.InternalServerError, InvalidPassword)
    {
    }
}
