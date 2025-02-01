using System.Threading.Tasks;
using Authorization.Application.Attributes;
using Authorization.Application.Constants;
using Authorization.Application.Models.Commands.Registration;
using Authorization.Application.Models.Requests.UserManagement;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Application.Controllers;

[ApiController]
[Route("registrations")]
public class RegistrationsController(
    IMediator mediator) : Controller
{
    /// <summary>
    /// Invite user.
    /// </summary>
    [HttpPost("invitations")]
    [Scope(Scopes.InviteUsers)]
    public async Task<IActionResult> InviteUser([FromBody] InviteUserRequestModel requestModel)
    {
        var result = await mediator.Send(new InviteUserCommand
        {
            Email = requestModel.Email,
            FirstName = requestModel.FirstName,
            LastName = requestModel.LastName,
        });

        return Ok(result);
    }
    
    /// <summary>
    /// Register user.
    /// </summary>
    [HttpPost("emails")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestModel requestModel)
    {
        var result = await mediator.Send(new RegisterUserCommand
        {
            Email = requestModel.Email,
            FirstName = requestModel.FirstName,
            LastName = requestModel.LastName,
            Password = requestModel.Password
        });

        return Ok(result);
    }
}