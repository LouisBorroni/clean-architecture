using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Authentication;

namespace TierListes.Application.UseCases.Authentication.Register;

public record RegisterCommand(string Email, string Username, string Password)
    : IRequest<Result<AuthResponseDto>>;
