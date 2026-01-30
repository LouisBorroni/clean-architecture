using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Configurations;

public class CompanyLogoConfiguration : IEntityTypeConfiguration<CompanyLogo>
{
    public void Configure(EntityTypeBuilder<CompanyLogo> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.LogoUrl)
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .IsRequired();
    }
}
