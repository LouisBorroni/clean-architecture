using TierListes.Domain.Entities;

namespace TierListes.Application.Common.Interfaces.Persistence;

public interface IPaymentHistoryRepository
{
    Task<IEnumerable<PaymentHistory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PaymentHistory?> GetByStripeSessionIdAsync(string sessionId, CancellationToken cancellationToken = default);
    Task AddAsync(PaymentHistory payment, CancellationToken cancellationToken = default);
}
