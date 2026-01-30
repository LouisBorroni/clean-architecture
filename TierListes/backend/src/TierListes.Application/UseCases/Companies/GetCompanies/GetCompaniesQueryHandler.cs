using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Company;

namespace TierListes.Application.UseCases.Companies.GetCompanies;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, Result<IEnumerable<CompanyDto>>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ILogoUrlGenerator _logoUrlGenerator;

    public GetCompaniesQueryHandler(
        ICompanyRepository companyRepository,
        ILogoUrlGenerator logoUrlGenerator)
    {
        _companyRepository = companyRepository;
        _logoUrlGenerator = logoUrlGenerator;
    }

    public async Task<Result<IEnumerable<CompanyDto>>> Handle(
        GetCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllWithLogosAsync(cancellationToken);

        var companyDtos = companies
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new CompanyDto(
                c.Id,
                c.Name,
                c.Logo?.LogoUrl ?? _logoUrlGenerator.GenerateLogoUrl(c.Domain)
            ));

        return Result<IEnumerable<CompanyDto>>.Success(companyDtos, 200);
    }
}
