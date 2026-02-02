using Microsoft.EntityFrameworkCore;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Repositories;

public class CompanyLogoRepository : ICompanyLogoRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyLogoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyLogo?> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos
            .FirstOrDefaultAsync(c => c.CompanyId == companyId, cancellationToken);
    }

    public async Task<bool> ExistsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos
            .AnyAsync(c => c.CompanyId == companyId, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos.CountAsync(cancellationToken);
    }

    public async Task AddAsync(CompanyLogo companyLogo, CancellationToken cancellationToken = default)
    {
        await _context.CompanyLogos.AddAsync(companyLogo, cancellationToken);
    }
}
