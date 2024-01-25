using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Authorization.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static IReadOnlyCollection<string> GetUserScopes(this ClaimsPrincipal principal)
    {
        var scopes = principal.Claims.Where(claim => claim.Type == "scopes")
            .Select(claimsRole => claimsRole.Value).ToList();

        return scopes;
    }

    public static Guid GetAuthorizedUserId(this ClaimsPrincipal principal)
    {
        var userId = principal.Claims.FirstOrDefault(firstName => firstName.Type == ClaimTypes.NameIdentifier)?.Value;
        return userId != null ? Guid.Parse(userId) : Guid.Empty;
    }
}
