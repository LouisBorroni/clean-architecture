using TierListes.Domain.Entities;

namespace TierListes.Application.Common.Interfaces.Persistence;

public interface ITierListRepository
{
    Task<IEnumerable<TierList>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TierList?> GetByUserAndCompanyAsync(Guid userId, Guid companyId, CancellationToken cancellationToken = default);
    Task AddAsync(TierList tierList, CancellationToken cancellationToken = default);
    Task DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
