using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Infrastructure.Configuration;

namespace TierListes.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    private readonly StripeSettings _settings;

    public StripePaymentService(IOptions<StripeSettings> settings)
    {
        _settings = settings.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public async Task<string> CreateCheckoutSessionAsync(Guid userId, string userEmail)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = _settings.PriceId,
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = _settings.SuccessUrl,
            CancelUrl = _settings.CancelUrl,
            CustomerEmail = userEmail,
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId.ToString() }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public async Task<WebhookValidationResult> ValidateWebhookAsync(string payload, string signature)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                payload,
                signature,
                _settings.WebhookSecret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session?.Metadata.TryGetValue("userId", out var userIdStr) == true &&
                    Guid.TryParse(userIdStr, out var userId))
                {
                    return new WebhookValidationResult(
                        IsValid: true,
                        UserId: userId,
                        SessionId: session.Id,
                        PaymentIntentId: session.PaymentIntentId,
                        Amount: session.AmountTotal.HasValue ? session.AmountTotal.Value / 100m : 0,
                        Currency: session.Currency ?? "eur",
                        Status: session.PaymentStatus ?? "completed"
                    );
                }
            }

            return new WebhookValidationResult(false, null, null, null, 0, "eur", "failed");
        }
        catch
        {
            return new WebhookValidationResult(false, null, null, null, 0, "eur", "error");
        }
    }
}
