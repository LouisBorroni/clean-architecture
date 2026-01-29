using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierListes.Application.DTOs.Company;
using TierListes.Application.UseCases.Companies.GetCompanies;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetCompaniesQuery();
        var result = await _mediator.Send(query);

        return Ok(result.Data);
    }
}
