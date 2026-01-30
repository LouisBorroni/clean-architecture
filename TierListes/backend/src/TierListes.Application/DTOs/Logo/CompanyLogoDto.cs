namespace TierListes.Application.DTOs.Logo;

public record CompanyLogoDto(
    Guid Id,
    string CompanyName,
    string LogoUrl,
    DateTime CreatedAt
);
