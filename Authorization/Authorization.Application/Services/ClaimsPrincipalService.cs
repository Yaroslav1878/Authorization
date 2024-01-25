using System.Linq;
using System.Security.Claims;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Authorization.Application.Services;

public class ClaimsPrincipalService(IHttpContextAccessor httpContextAccessor) : IClaimsPrincipalService
{
    private readonly ClaimsPrincipal? _claimsPrincipal = httpContextAccessor.HttpContext?.User;

    private ClaimsPrincipal ClaimsPrincipal
    {
        get
        {
            if (_claimsPrincipal == null)
            {
                throw new AuthorizationException();
            }

            return _claimsPrincipal;
        }
    }

    public int GetAuthorizedUserId()
    {
        var userId = ClaimsPrincipal.Claims.FirstOrDefault(firstName => firstName.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new AuthorizationException();
        }

        return int.Parse(userId);
    }

    public bool HasScope(string scope)
    {
        var scopes = ClaimsPrincipal.Claims.Where(claim => claim.Type == "scopes")
            .Select(claimsRole => claimsRole.Value).ToList();

        return scopes.Contains(scope);
    }
}
