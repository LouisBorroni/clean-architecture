namespace TierListes.Application.DTOs.Authentication;

public record AuthResponseDto(Guid UserId, string Email, string Username, string Token);
