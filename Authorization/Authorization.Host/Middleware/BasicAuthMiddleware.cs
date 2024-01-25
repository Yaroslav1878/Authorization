using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Application.Constants;
using Microsoft.AspNetCore.Http;

namespace Authorization.Host.Middleware
{
    public class BasicAuthMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// BasicAuthMiddleware.
        /// </summary>
        /// <remarks>
        /// Middleware for adding a claims principal to the user's context.
        /// </remarks>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string? accessToken;

            // todo: make a better approach of just using cookie
            if (httpContext.Request.Headers.Referer.ToString().Contains("/swagger/index.html"))
            {
                accessToken = httpContext.Request.Headers["Authorization"].ToString().Split(new[] { ' ' }).Last();
            }
            else
            {
                accessToken = httpContext.Request.Cookies[CookieNames.AccessToken] ??
                              httpContext.Request.Headers["Authorization"].ToString().Split(new[] { ' ' }).Last(); // temporary solution
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.ReadToken(accessToken) is JwtSecurityToken securityToken)
                {
                    var scopes = securityToken.Claims.Where(claim => claim.Type == "scopes").ToList();
                    var roles = securityToken.Claims.Where(claim => claim.Type == "role").ToList();

                    var claimCollection = new List<Claim>
                    {
                        new (ClaimTypes.NameIdentifier, securityToken.Claims.First(claim => claim.Type == "nameid").Value),
                        new (ClaimTypes.Name, securityToken.Claims.First(claim => claim.Type == "unique_name").Value),
                        new (ClaimTypes.Surname, securityToken.Claims.First(claim => claim.Type == "family_name").Value),
                        new (ClaimTypes.Email, securityToken.Claims.First(claim => claim.Type == "email").Value),
                        new (ClaimTypes.Expired, securityToken.Claims.First(claim => claim.Type == "exp").Value),
                    };
                    claimCollection.AddRange(roles);
                    claimCollection.AddRange(scopes);

                    var claimsIdentity = new ClaimsIdentity(claimCollection, "Profile");
                    var principal = new ClaimsPrincipal(claimsIdentity);

                    httpContext.User = principal;

                    await next(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                await next(httpContext);
            }
        }
    }
}
