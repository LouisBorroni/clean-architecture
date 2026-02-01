namespace TierListes.Domain.Entities;

public class PaymentHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string StripeSessionId { get; set; } = string.Empty;
    public string? StripePaymentIntentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "eur";
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;

    public static PaymentHistory Create(
        Guid userId,
        string stripeSessionId,
        string? stripePaymentIntentId,
        decimal amount,
        string currency,
        string status)
    {
        return new PaymentHistory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StripeSessionId = stripeSessionId,
            StripePaymentIntentId = stripePaymentIntentId,
            Amount = amount,
            Currency = currency,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }
}
