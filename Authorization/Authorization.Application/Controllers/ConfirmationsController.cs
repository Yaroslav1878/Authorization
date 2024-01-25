using System;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Application.Models.Requests.Confirmation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Application.Controllers;

[ApiController]
[Route("confirmations")]
public class ConfirmationsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Validate confirmation request.
    /// </summary>
    [HttpGet("{confirmationId}")]
    public async Task<IActionResult> ValidateConfirmationRequest(
        [FromRoute] Guid confirmationId,
        [FromQuery] string hash)
    {
        var request = new ValidateConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Hash = hash,
        };
        await mediator.Send(request);
        return Ok();
    }
    
    /// <summary>
    /// Send password recovery email.
    /// </summary>
    [HttpPost("passwords")]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordRecoveryRequestModel requestModel)
    {
        await mediator.Send(new PasswordRecoveryCommand { Email = requestModel.Email });
        return NoContent();
    }

    /// <summary>
    /// Confirm password.
    /// </summary>
    [HttpPatch("{confirmationId}/passwords")]
    public async Task<IActionResult> ConfirmPasswordReset(
        [FromRoute] Guid confirmationId,
        [FromQuery] string hash,
        [FromBody] PasswordConfirmationRequestModel requestModel)
    {
        await mediator.Send(new ValidateConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Hash = hash,
        });
        var request = new PasswordConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Password = requestModel.NewPassword,
        };
        await mediator.Send(request);
        return NoContent();
    }
    
    [HttpPatch("{confirmationId}/invitations")]
    public async Task<IActionResult> ConfirmInvitation(
        [FromRoute] Guid confirmationId,
        [FromQuery] string hash,
        [FromBody] InviteConfirmationRequestModel requestModel)
    {
        await mediator.Send(new ValidateConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Hash = hash,
        });
        var request = new InviteConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Password = requestModel.Password,
        };
        await mediator.Send(request);
        return NoContent();
    }
    
    [HttpPatch("{confirmationId}/registration")]
    public async Task<IActionResult> ConfirmRegistration(
        [FromRoute] Guid confirmationId,
        [FromQuery] string hash)
    {
        await mediator.Send(new ValidateConfirmationCommand
        {
            ConfirmationId = confirmationId,
            Hash = hash,
        });
        var request = new RegistrationConfirmationCommand
        {
            ConfirmationId = confirmationId,
        };
        await mediator.Send(request);
        return NoContent();
    }
    
    /// <summary>
    /// Resend confirmation.
    /// </summary>
    [HttpPost("{userId}/resend")]
    public async Task<IActionResult> ResendConfirmation([FromRoute] int userId)
    {
        await mediator.Send(new ConfirmResendCommand
        {
            UserId = userId,
        });
        return NoContent();
    }
}
