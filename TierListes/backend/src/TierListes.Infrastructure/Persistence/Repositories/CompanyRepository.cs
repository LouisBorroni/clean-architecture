using Microsoft.EntityFrameworkCore;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Company>> GetAllWithLogosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .Include(c => c.Logo)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Company?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .Include(c => c.Logo)
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AnyAsync(c => c.Name == name, cancellationToken);
    }

    public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
    {
        await _context.Companies.AddAsync(company, cancellationToken);
    }

    public async Task<int> GetMaxDisplayOrderAsync(CancellationToken cancellationToken = default)
    {
        if (!await _context.Companies.AnyAsync(cancellationToken))
            return 0;

        return await _context.Companies.MaxAsync(c => c.DisplayOrder, cancellationToken);
    }
}
