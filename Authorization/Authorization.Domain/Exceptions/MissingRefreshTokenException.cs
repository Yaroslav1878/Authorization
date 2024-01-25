using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class MissingRefreshTokenException : ApplicationException
{
    private const string ErrorMissingRefreshToken = "Refresh token is missing.";

    public MissingRefreshTokenException()
        : base(ErrorCode.MissingRefreshTokenException, HttpStatusCode.Unauthorized, ErrorMissingRefreshToken)
    {
    }
}
