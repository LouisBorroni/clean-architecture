using Microsoft.EntityFrameworkCore;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<TierList> TierLists { get; set; }
    public DbSet<CompanyLogo> CompanyLogos { get; set; }
    public DbSet<PaymentHistory> PaymentHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
