using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions
{
    public class ClientAuthorizationException : ApplicationException
    {
        private const string MessageTemplate = "Client auth failed ('client_id' or 'client_secret' is invalid).";

        public ClientAuthorizationException()
            : base(ErrorCode.ClientAuthorizationException, HttpStatusCode.Unauthorized, MessageTemplate)
        {
        }
    }
}
