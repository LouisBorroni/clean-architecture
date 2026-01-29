using Microsoft.EntityFrameworkCore;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Repositories;

public class TierListRepository : ITierListRepository
{
    private readonly ApplicationDbContext _context;

    public TierListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TierList>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.TierLists
            .Where(tl => tl.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TierList?> GetByUserAndCompanyAsync(Guid userId, Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _context.TierLists
            .FirstOrDefaultAsync(tl => tl.UserId == userId && tl.CompanyId == companyId, cancellationToken);
    }

    public async Task AddAsync(TierList tierList, CancellationToken cancellationToken = default)
    {
        await _context.TierLists.AddAsync(tierList, cancellationToken);
    }

    public async Task DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tierLists = await _context.TierLists
            .Where(tl => tl.UserId == userId)
            .ToListAsync(cancellationToken);

        _context.TierLists.RemoveRange(tierLists);
    }
}
