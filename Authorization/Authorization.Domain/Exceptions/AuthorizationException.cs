using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions
{
    public class AuthorizationException : ApplicationException
    {
        private const string AccessTokenExpired = "The access token has expired.";

        public AuthorizationException()
            : base(ErrorCode.AuthorizationException, HttpStatusCode.Unauthorized, AccessTokenExpired)
        {
        }
    }
}
