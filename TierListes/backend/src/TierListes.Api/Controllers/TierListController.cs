using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierListes.Application.DTOs.TierList;
using TierListes.Application.UseCases.TierLists.ExportTierList;
using TierListes.Application.UseCases.TierLists.GetTierList;
using TierListes.Application.UseCases.TierLists.SaveTierList;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TierListController : ControllerBase
{
    private readonly IMediator _mediator;

    public TierListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TierListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var query = new GetTierListQuery(userId.Value);
        var result = await _mediator.Send(query);

        return Ok(result.Data);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Save([FromBody] SaveTierListRequestDto request)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new SaveTierListCommand(userId.Value, request.Rankings);
        var result = await _mediator.Send(command);

        return Ok(new { success = result.Data });
    }

    [HttpPost("export")]
    [ProducesResponseType(typeof(ExportResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Export([FromBody] ExportTierListRequestDto request)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var imageData = Convert.FromBase64String(request.ImageBase64);
        var command = new ExportTierListCommand(userId.Value, imageData);
        var result = await _mediator.Send(command);

        return Ok(new ExportResponseDto(result.Data!));
    }

    private Guid? GetUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return null;
        }

        return userId;
    }
}
