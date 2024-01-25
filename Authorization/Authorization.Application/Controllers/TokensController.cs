using System.Threading.Tasks;
using Authorization.Application.Constants;
using Authorization.Application.Models.Commands.Token;
using Authorization.Application.Models.Requests.Token;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Application.Controllers;

[ApiController]
[Route("tokens")]

public class TokensController(
    ICookieService cookieService,
    IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Issue token.
    /// </summary>
    [HttpPost("credentials")]
    public async Task<IActionResult> IssueToken([FromBody] IssueTokenCommand command)
    {
        var response = await mediator.Send(command);
        cookieService.SetAccessTokenCookie(CookieNames.AccessToken, response.AccessTokenResponseModel.AccessToken);
        cookieService.SetAccessTokenCookie(CookieNames.RefreshToken, response.RefreshTokenResponseModel.RefreshToken);
        return Ok(response.AccessTokenResponseModel);
    }

    /// <summary>
    /// Refresh access token.
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenRequestModel request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new MissingRefreshTokenException();
        }

        var command = new RefreshTokenCommand
        {
            RefreshToken = request.RefreshToken,
        };

        var response = await mediator.Send(command);
        cookieService.SetAccessTokenCookie(CookieNames.AccessToken, response.AccessTokenResponseModel.AccessToken);
        cookieService.SetAccessTokenCookie(CookieNames.RefreshToken, response.RefreshTokenResponseModel.RefreshToken);
        return Ok(response.AccessTokenResponseModel);
    }

    /// <summary>
    /// Logout.
    /// </summary>
    [HttpDelete("me")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestModel request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return BadRequest(MessageTemplates.ErrorMissingRefreshToken);
        }

        var command = new LogoutCommand
        {
            RefreshToken = request.RefreshToken,
        };
        Response.Cookies.Delete(CookieNames.AccessToken);
        Response.Cookies.Delete(CookieNames.RefreshToken);
        await mediator.Send(command);
        return NoContent();
    }
}
