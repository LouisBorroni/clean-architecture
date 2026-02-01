using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Application.DTOs.Payment;
using TierListes.Domain.Entities;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IUserRepository _userRepository;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentController(
        IPaymentService paymentService,
        IUserRepository userRepository,
        IPaymentHistoryRepository paymentHistoryRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentService = paymentService;
        _userRepository = userRepository;
        _paymentHistoryRepository = paymentHistoryRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("checkout")]
    [Authorize]
    [ProducesResponseType(typeof(CheckoutResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCheckoutSession()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized();
        }

        if (user.HasPaid)
        {
            return BadRequest(new { error = "Vous avez déjà payé." });
        }

        var checkoutUrl = await _paymentService.CreateCheckoutSessionAsync(userId, user.Email);
        return Ok(new CheckoutResponseDto(checkoutUrl));
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].ToString();

        var result = await _paymentService.ValidateWebhookAsync(json, signature);

        if (result.IsValid && result.UserId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(result.UserId.Value);
            if (user != null)
            {
                user.MarkAsPaid();

                var paymentHistory = PaymentHistory.Create(
                    result.UserId.Value,
                    result.SessionId!,
                    result.PaymentIntentId,
                    result.Amount,
                    result.Currency,
                    result.Status
                );
                await _paymentHistoryRepository.AddAsync(paymentHistory);

                await _unitOfWork.SaveChangesAsync();
            }
        }

        return Ok();
    }

    [HttpGet("status")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPaymentStatus()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new { hasPaid = user.HasPaid });
    }
}
