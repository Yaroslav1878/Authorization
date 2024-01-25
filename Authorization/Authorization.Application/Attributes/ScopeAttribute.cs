using System;
using System.Linq;
using Authorization.Application.Extensions;
using Authorization.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authorization.Application.Attributes;

public class ScopeAttribute(params string[] resourceRequiredScopes) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext?.User.Identity?.IsAuthenticated == false)
        {
            throw new AuthorizationException();
        }

        var scopes = context.HttpContext?.User.GetUserScopes();
        bool userHasPermission = scopes!.Any(jwtScopes => resourceRequiredScopes.Any(userScopes => userScopes == jwtScopes));
        if (userHasPermission)
        {
            return;
        }

        context.Result = new JsonResult(new { message = "Forbidden" })
            { StatusCode = StatusCodes.Status403Forbidden };
    }
}
