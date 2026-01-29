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

        builder.HasData(
            new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Nintendo", Domain = "nintendo.com", DisplayOrder = 1 },
            new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "PlayStation", Domain = "playstation.com", DisplayOrder = 2 },
            new { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Xbox", Domain = "xbox.com", DisplayOrder = 3 },
            new { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "EA", Domain = "ea.com", DisplayOrder = 4 },
            new { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Ubisoft", Domain = "ubisoft.com", DisplayOrder = 5 },
            new { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Activision Blizzard", Domain = "activisionblizzard.com", DisplayOrder = 6 },
            new { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Rockstar Games", Domain = "rockstargames.com", DisplayOrder = 7 },
            new { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "Square Enix", Domain = "square-enix.com", DisplayOrder = 8 },
            new { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "Capcom", Domain = "capcom.com", DisplayOrder = 9 },
            new { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Epic Games", Domain = "epicgames.com", DisplayOrder = 10 }
        );
    }
}
