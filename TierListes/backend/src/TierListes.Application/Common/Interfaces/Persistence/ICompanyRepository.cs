using TierListes.Domain.Entities;

namespace TierListes.Application.Common.Interfaces.Persistence;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
