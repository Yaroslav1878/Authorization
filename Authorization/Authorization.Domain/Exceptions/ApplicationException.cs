using System;
using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public abstract class ApplicationException(
        ErrorCode errorCode,
        HttpStatusCode statusCode,
        string? message) : Exception(message)
{
    public ErrorCode ErrorCodeValue { get; } = errorCode;
    public HttpStatusCode StatusCode { get; } = statusCode;
}
