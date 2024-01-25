using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class PermissionException : ApplicationException
{
    private const string ForbiddenMessage = "User does not have permission";

    public PermissionException()
        : base(ErrorCode.PermissionException, HttpStatusCode.Forbidden, ForbiddenMessage)
    {
    }
}
