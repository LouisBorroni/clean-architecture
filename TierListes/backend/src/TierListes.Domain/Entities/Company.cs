namespace TierListes.Domain.Entities;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public CompanyLogo? Logo { get; set; }

    private Company() { }

    public static Company Create(string name, string domain, int displayOrder)
    {
        return new Company
        {
            Id = Guid.NewGuid(),
            Name = name,
            Domain = domain,
            DisplayOrder = displayOrder
        };
    }
}
