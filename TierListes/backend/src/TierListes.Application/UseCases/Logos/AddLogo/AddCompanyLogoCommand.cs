using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Logo;

namespace TierListes.Application.UseCases.Logos.AddLogo;

public record AddCompanyLogoCommand(string CompanyName, string LogoUrl)
    : IRequest<Result<CompanyLogoDto>>;
