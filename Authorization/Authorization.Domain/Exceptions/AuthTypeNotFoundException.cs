using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class AuthTypeNotFoundException : ApplicationException
{
    private const string AuthTypeNotFound = "Auth type not found by: ";

    public AuthTypeNotFoundException(AuthType authType)
        : base(ErrorCode.AuthTypeNotFoundException, HttpStatusCode.NotFound, AuthTypeNotFound + authType)
    {
    }
}
