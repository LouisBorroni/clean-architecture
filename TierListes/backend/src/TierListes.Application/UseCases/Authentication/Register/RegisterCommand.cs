using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Authentication;

public record RegisterCommand(string Email, string Username, string Password)
    : IRequest<Result<AuthResponseDto>>;
