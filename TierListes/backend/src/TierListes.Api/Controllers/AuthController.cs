using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierListes.Application.DTOs.Authentication;
using TierListes.Application.UseCases.Authentication.Login;
using TierListes.Application.UseCases.Authentication.Register;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _env;
    private const string AuthCookieName = "auth_token";

    public AuthController(IMediator mediator, IWebHostEnvironment env)
    {
        _mediator = mediator;
        _env = env;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var command = new RegisterCommand(dto.Email, dto.Username, dto.Password);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
        }

        SetAuthCookie(result.Data!.Token);

        var userResponse = new UserResponseDto(
            result.Data.UserId,
            result.Data.Email,
            result.Data.Username
        );

        return StatusCode(result.StatusCode, userResponse);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto.Email, dto.Password);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
        }

        SetAuthCookie(result.Data!.Token);

        var userResponse = new UserResponseDto(
            result.Data.UserId,
            result.Data.Email,
            result.Data.Username
        );

        return Ok(userResponse);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthCookieName, GetCookieOptions());
        return Ok(new { message = "Déconnexion réussie" });
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        var username = User.FindFirst("username")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        return Ok(new UserResponseDto(
            Guid.Parse(userId),
            email ?? "",
            username ?? ""
        ));
    }

    private void SetAuthCookie(string token)
    {
        var cookieOptions = GetCookieOptions();
        cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(1);
        Response.Cookies.Append(AuthCookieName, token, cookieOptions);
    }

    private CookieOptions GetCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite = _env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
            Path = "/"
        };
    }
}
