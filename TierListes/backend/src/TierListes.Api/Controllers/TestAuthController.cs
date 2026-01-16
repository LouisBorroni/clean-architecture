using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestAuthController : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var username = User.FindFirst("username")?.Value;

        return Ok(
            new
            {
                userId,
                email,
                username,
                message = "Vous êtes authentifié !"
            }
        );
    }
}
