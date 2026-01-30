using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TierListes.Domain.Entities;
using TierListes.Infrastructure.Configuration;

namespace TierListes.Infrastructure.Persistence;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly string _logoApiToken;

    public DbInitializer(ApplicationDbContext context, IOptions<LogoSettings> logoSettings)
    {
        _context = context;
        _logoApiToken = logoSettings.Value.ApiToken;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        if (await _context.Companies.AnyAsync())
            return;

        var companies = new List<(string Name, string Domain)>
        {
            ("Nintendo", "nintendo.com"),
            ("PlayStation", "playstation.com"),
            ("Xbox", "xbox.com"),
            ("EA", "ea.com"),
            ("Ubisoft", "ubisoft.com"),
            ("Activision Blizzard", "activisionblizzard.com"),
            ("Rockstar Games", "rockstargames.com"),
            ("Square Enix", "square-enix.com"),
            ("Capcom", "capcom.com"),
            ("Epic Games", "epicgames.com")
        };

        var displayOrder = 1;
        foreach (var (name, domain) in companies)
        {
            var company = Company.Create(name, domain, displayOrder++);
            await _context.Companies.AddAsync(company);

            var logo = CompanyLogo.Create(company.Id, $"https://img.logo.dev/{domain}?token={_logoApiToken}");
            await _context.CompanyLogos.AddAsync(logo);
        }

        await _context.SaveChangesAsync();
    }
}
