namespace TierListes.Domain.Entities;

public class CompanyLogo
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Company Company { get; set; } = null!;

    private CompanyLogo() { }

    public static CompanyLogo Create(Guid companyId, string logoUrl)
    {
        return new CompanyLogo
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            LogoUrl = logoUrl,
            CreatedAt = DateTime.UtcNow
        };
    }
}
