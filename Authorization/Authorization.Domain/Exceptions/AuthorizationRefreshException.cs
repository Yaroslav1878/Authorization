using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class AuthorizationRefreshException : ApplicationException
{
    private const string RefreshTokenExpired = "The session has expired.";

    public AuthorizationRefreshException()
        : base(ErrorCode.AuthorizationRefreshException, HttpStatusCode.Unauthorized, RefreshTokenExpired)
    {
    }
}
