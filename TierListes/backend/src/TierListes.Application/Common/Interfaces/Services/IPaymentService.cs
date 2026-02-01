namespace TierListes.Application.Common.Interfaces.Services;

public record WebhookValidationResult(
    bool IsValid,
    Guid? UserId,
    string? SessionId,
    string? PaymentIntentId,
    decimal Amount,
    string Currency,
    string Status
);

public interface IPaymentService
{
    Task<string> CreateCheckoutSessionAsync(Guid userId, string userEmail);
    Task<WebhookValidationResult> ValidateWebhookAsync(string payload, string signature);
}
