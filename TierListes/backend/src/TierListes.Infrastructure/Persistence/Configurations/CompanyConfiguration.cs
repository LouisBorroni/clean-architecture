using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Domain)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.DisplayOrder)
            .IsRequired();

        builder.HasOne(c => c.Logo)
            .WithOne(l => l.Company)
            .HasForeignKey<CompanyLogo>(l => l.CompanyId);
    }
}
