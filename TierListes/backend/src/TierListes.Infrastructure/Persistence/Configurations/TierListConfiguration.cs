using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Configurations;

public class TierListConfiguration : IEntityTypeConfiguration<TierList>
{
    public void Configure(EntityTypeBuilder<TierList> builder)
    {
        builder.HasKey(tl => tl.Id);

        builder.Property(tl => tl.TierLevel)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(tl => tl.User)
            .WithMany()
            .HasForeignKey(tl => tl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tl => tl.Company)
            .WithMany()
            .HasForeignKey(tl => tl.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tl => new { tl.UserId, tl.CompanyId })
            .IsUnique();
    }
}
