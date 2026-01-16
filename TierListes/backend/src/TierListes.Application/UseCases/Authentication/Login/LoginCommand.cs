using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.Authentication;

namespace TierListes.Application.UseCases.Authentication.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
