using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(ErrorCode errorCode, string message)
        : base(errorCode, HttpStatusCode.BadRequest, message)
    {
    }
}
