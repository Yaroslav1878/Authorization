using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Application.Attributes;
using Authorization.Application.Constants;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Application.Models.Commands.User;
using Authorization.Application.Models.Commands.UserManagement;
using Authorization.Application.Models.Requests.User;
using Authorization.Application.Services.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Application.Controllers;

[ApiController]
[Route("users")]
public class UsersController(
        IMediator mediator,
        IClaimsPrincipalService claimsPrincipalService) : ControllerBase
{
    /// <summary>
    /// Get own profile.
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetOwnProfile()
    {
        var userId = claimsPrincipalService.GetAuthorizedUserId();

        var request = new GetUserCommand
        {
            UserId = userId,
        };

        var response = await mediator.Send(request);
        return Ok(response);
    }

    /// <summary>
    /// Update own profile.
    /// </summary>
    [HttpPut("me")]
    public async Task<IActionResult> UpdateOwnProfile([FromBody] UpdateOwnProfileRequestModel updateOwnProfileRequestModel)
    {
        int userId = claimsPrincipalService.GetAuthorizedUserId();

        var result = await mediator.Send(new UpdateUserCommand
        {
            UserId = userId,
            FirstName = updateOwnProfileRequestModel.FirstName,
            LastName = updateOwnProfileRequestModel.LastName,
        });

        return Ok(result);
    }

    /// <summary>
    /// Change own password.
    /// </summary>
    [HttpPatch("me/password/{confirmationId}")]
    public async Task<IActionResult> UpdatePassword(
        [FromRoute] Guid confirmationId,
        [FromQuery] string hash,
        [FromBody] UpdatePasswordRequestModel requestModel)
    {
        await mediator.Send(new ValidateConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Hash = hash,
        });

        var response = await mediator.Send(new UpdateUserPasswordCommand
        {
            ConfirmationId = confirmationId,
            NewPassword = requestModel.NewPassword,
        });

        return Ok(response);
    }
    
    /// <summary>
    /// Update own avatar.
    /// </summary>
    [HttpPatch("me/avatar")]
    public async Task<IActionResult> UpdateOwnAvatar(
        [FromBody] UpdateOwnAvatarRequestModel requestModel)
    {
        int userId = claimsPrincipalService.GetAuthorizedUserId();
        
        var response = await mediator.Send(new UpdateOwnAvatarCommand
        {
            UserId = userId,
            AvatarId = requestModel.AvatarId
        });

        return Ok(response);
    }
    
    /// <summary>
    /// Delete own avatar.
    /// </summary>
    [HttpDelete("me/avatar")]
    public async Task<IActionResult> DeleteOwnAvatar()
    {
        int userId = claimsPrincipalService.GetAuthorizedUserId();

        var response = await mediator.Send(new DeleteOwnAvatarCommand
        {
            UserId = userId
        });

        return Ok(response);
    }

    [HttpPut("{userId}")]
    [Scope(Scopes.ManageUsers)]
    public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserRequestModel request)
    {
        var result = await mediator.Send(new UpdateUserCommand
        {
            UserId = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
        });
        return Ok(result);
    }

    /// <summary>
    /// Get users.
    /// </summary>
    [HttpGet]
    [Scope(Scopes.ViewUser, Scopes.ManageUsers)]
    public async Task<IActionResult> GetUsers([FromQuery] HashSet<int> userIds, [FromQuery] List<string> rolesFilter)
    {
        var response = await mediator.Send(new GetUsersCommand
        {
            UserIds = userIds,
            RolesFilter = rolesFilter,
        });
        return Ok(response);
    }

    /// <summary>
    /// Get user.
    /// </summary>
    [HttpGet("{userId}")]
    [Scope(Scopes.ViewUser, Scopes.ManageUsers)]
    public async Task<IActionResult> GetUserById([FromRoute] int userId)
    {
        var response = await mediator.Send(new GetUserCommand
        {
            UserId = userId,
        });
        return Ok(response);
    }

    //todo: remove if not needed
    /// <summary>
    /// Update user roles.
    /// </summary>
    [HttpPut("{userId}/roles")]
    [Scope(Scopes.ManageUsers)]
    public async Task<IActionResult> UpdateUserRoles(
        [FromRoute] int userId,
        [FromBody] UpdateUserRolesRequestModel request)
    {
        var response = await mediator.Send(new UpdateUserRolesCommand
        {
            UserId = userId,
            RoleIds = request.RoleIds,
        });
        return Ok(response);
    }

    /// <summary>
    /// Update user status.
    /// </summary>
    [HttpPut("{userId}/status")]
    [Scope(Scopes.ManageUsers)]
    public async Task<IActionResult> UpdateUserStatus([FromRoute] int userId, [FromBody] UpdateUserStatusRequestModel request)
    {
        var response = await mediator.Send(new UpdateUserStatusCommand
        {
            UserId = userId,
            Status = request.Status,
        });
        return Ok(response);
    }

    /// <summary>
    /// Delete user.
    /// </summary>
    [HttpDelete("{userId}")]
    [Scope(Scopes.ManageUsers)]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        var response = await mediator.Send(new DeleteUserCommand
        {
            UserId = userId,
        });
        return Ok(response);
    }
}
