using System.Globalization;
using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions
{
    public class SendGridException : ApplicationException
    {
        private const string MessageTemplate = "Please, check exceptions by status code.";

        public SendGridException(HttpStatusCode statusCode)
            : base(ErrorCode.SendGridException, statusCode, string.Format(CultureInfo.InvariantCulture, MessageTemplate))
        {
        }
    }
}
