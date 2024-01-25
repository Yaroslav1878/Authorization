using System.Globalization;
using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class ConfirmationWasUsedException : ApplicationException
{
    private const string MessageTemplate = "Confirmation is not valid anymore.";

    public ConfirmationWasUsedException()
        : base(ErrorCode.UserAlreadyConfirmedException, HttpStatusCode.Found, string.Format(CultureInfo.InvariantCulture, MessageTemplate))
    {
    }
}
