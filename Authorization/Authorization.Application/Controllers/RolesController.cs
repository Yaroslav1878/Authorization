using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Application.Controllers;

[ApiController]
[Route("roles")]
public class RolesController(IMediator mediator) : Controller
{
    /// <summary>
    /// Get roles.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await mediator.Send(new GetRolesCommand());

        return Ok(roles);
    }

    /// <summary>
    /// Get assignable roles.
    /// </summary>
    [HttpGet("assignable")]
    public async Task<IActionResult> GetAssignableRoles([FromQuery] int? userId)
    {
        var roles = await mediator.Send(new GetAssignableRolesCommand { UserId = userId });

        return Ok(roles);
    }
}
