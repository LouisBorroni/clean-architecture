namespace TierListes.Domain.Entities;

public class TierList
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public string TierLevel { get; set; } = string.Empty;
    public int Position { get; set; }

    public User User { get; set; } = null!;
    public Company Company { get; set; } = null!;

    private TierList() { }

    public static TierList Create(Guid userId, Guid companyId, string tierLevel, int position = 0)
    {
        return new TierList
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CompanyId = companyId,
            TierLevel = tierLevel,
            Position = position
        };
    }

    public void UpdateTierLevel(string tierLevel, int position)
    {
        TierLevel = tierLevel;
        Position = position;
    }
}
