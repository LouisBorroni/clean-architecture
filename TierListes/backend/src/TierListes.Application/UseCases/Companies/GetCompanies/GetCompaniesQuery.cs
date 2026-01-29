using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Company;

namespace TierListes.Application.UseCases.Companies.GetCompanies;

public record GetCompaniesQuery() : IRequest<Result<IEnumerable<CompanyDto>>>;
