using System.Globalization;
using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class ConfirmationRequestExpiredException : ApplicationException
{
    private const string MessageTemplate = "Confirmation request is expired.";

    public ConfirmationRequestExpiredException()
        : base(ErrorCode.ConfirmationRequestExpiredException, HttpStatusCode.Found, string.Format(CultureInfo.InvariantCulture, MessageTemplate))
    {
    }
}
