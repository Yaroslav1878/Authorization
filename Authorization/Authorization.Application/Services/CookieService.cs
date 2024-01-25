using System.Collections.Generic;
using Authorization.Application.Constants;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Authorization.Application.Services;

public class CookieService(
        IHttpContextAccessor httpContextAccessor,
        ITimeProviderService timeProviderService,
        IJwtConfiguration jwtConfiguration) : ICookieService
{
    public void SetAccessTokenCookie(string cookieNames, string value)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(
            cookieNames,
            value,
            new CookieOptions
            {
                Expires = timeProviderService.UtcNow.AddMinutes(jwtConfiguration.ExpirationTimeInMinutes),
                SameSite = SameSiteMode.None,
                Secure = true,
            });
    }
}
