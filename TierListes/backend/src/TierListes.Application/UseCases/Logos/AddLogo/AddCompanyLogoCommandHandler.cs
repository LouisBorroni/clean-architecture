using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Logo;
using TierListes.Domain.Entities;

namespace TierListes.Application.UseCases.Logos.AddLogo;

public class AddCompanyLogoCommandHandler : IRequestHandler<AddCompanyLogoCommand, Result<CompanyLogoDto>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyLogoRepository _companyLogoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCompanyLogoCommandHandler(
        ICompanyRepository companyRepository,
        ICompanyLogoRepository companyLogoRepository,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _companyLogoRepository = companyLogoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CompanyLogoDto>> Handle(
        AddCompanyLogoCommand request,
        CancellationToken cancellationToken)
    {
        var logoCount = await _companyLogoRepository.CountAsync(cancellationToken);
        if (logoCount >= 10)
        {
            return Result<CompanyLogoDto>.Failure(
                "Le nombre maximum de 10 logos a été atteint.", 400
            );
        }

        var existingCompany = await _companyRepository.GetByNameAsync(request.CompanyName, cancellationToken);

        if (existingCompany != null && existingCompany.Logo != null)
        {
            return Result<CompanyLogoDto>.Conflict(
                $"Un logo pour l'entreprise '{request.CompanyName}' existe déjà."
            );
        }

        Company company;
        if (existingCompany != null)
        {
            company = existingCompany;
        }
        else
        {
            var maxDisplayOrder = await _companyRepository.GetMaxDisplayOrderAsync(cancellationToken);
            company = Company.Create(
                name: request.CompanyName,
                domain: string.Empty,
                displayOrder: maxDisplayOrder + 1
            );
            await _companyRepository.AddAsync(company, cancellationToken);
        }

        var companyLogo = CompanyLogo.Create(company.Id, request.LogoUrl);
        await _companyLogoRepository.AddAsync(companyLogo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CompanyLogoDto(
            Id: companyLogo.Id,
            CompanyName: company.Name,
            LogoUrl: companyLogo.LogoUrl,
            CreatedAt: companyLogo.CreatedAt
        );

        return Result<CompanyLogoDto>.Success(response, 201);
    }
}
