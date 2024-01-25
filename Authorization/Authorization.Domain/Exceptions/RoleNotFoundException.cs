using System.Globalization;
using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class RoleNotFoundException : ApplicationException
{
    private const string MessageTemplate = "Role with name '{0}' not found.";

    public RoleNotFoundException(string roleName)
        : base(ErrorCode.RoleNotFoundException, HttpStatusCode.NotFound, string.Format(CultureInfo.InvariantCulture, MessageTemplate, roleName))
    {
    }
}
