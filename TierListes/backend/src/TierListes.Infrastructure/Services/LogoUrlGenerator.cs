using Microsoft.Extensions.Options;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Infrastructure.Configuration;

namespace TierListes.Infrastructure.Services;

public class LogoUrlGenerator : ILogoUrlGenerator
{
    private readonly LogoSettings _settings;

    public LogoUrlGenerator(IOptions<LogoSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateLogoUrl(string domain)
    {
        return $"https://img.logo.dev/{domain}?token={_settings.ApiToken}";
    }
}
