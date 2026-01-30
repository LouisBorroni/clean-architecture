using MediatR;
using Microsoft.AspNetCore.Mvc;
using TierListes.Application.DTOs.Logo;
using TierListes.Application.UseCases.Logos.AddLogo;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogoController : ControllerBase
{
    private readonly IMediator _mediator;

    public LogoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CompanyLogoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddLogo([FromBody] AddCompanyLogoRequest request)
    {
        var command = new AddCompanyLogoCommand(request.CompanyName, request.LogoUrl);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }

        return StatusCode(result.StatusCode, result.Data);
    }
}

public record AddCompanyLogoRequest(string CompanyName, string LogoUrl);
