using TierListes.Domain.Entities;

namespace TierListes.Application.Common.Interfaces.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
