using Microsoft.EntityFrameworkCore;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Repositories;

public class PaymentHistoryRepository : IPaymentHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentHistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaymentHistory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.PaymentHistories
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaymentHistory?> GetByStripeSessionIdAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await _context.PaymentHistories
            .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId, cancellationToken);
    }

    public async Task AddAsync(PaymentHistory payment, CancellationToken cancellationToken = default)
    {
        await _context.PaymentHistories.AddAsync(payment, cancellationToken);
    }
}
