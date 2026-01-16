namespace TierListes.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Passwordhash { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    private User() { }

    public static User Create(string email, string username, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = username,
            Passwordhash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}
