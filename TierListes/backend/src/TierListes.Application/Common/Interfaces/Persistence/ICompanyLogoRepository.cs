using TierListes.Domain.Entities;

namespace TierListes.Application.Common.Interfaces.Persistence;

public interface ICompanyLogoRepository
{
    Task<CompanyLogo?> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CompanyLogo companyLogo, CancellationToken cancellationToken = default);
}
