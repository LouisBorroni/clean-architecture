using TierListes.Domain.Entities;

namespace TierListes.Application.Common.Interfaces.Persistence;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Company>> GetAllWithLogosAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Company?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    Task<int> GetMaxDisplayOrderAsync(CancellationToken cancellationToken = default);
}
