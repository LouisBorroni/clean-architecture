using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierListes.Domain.Entities;

namespace TierListes.Infrastructure.Persistence.Configurations;

public class PaymentHistoryConfiguration : IEntityTypeConfiguration<PaymentHistory>
{
    public void Configure(EntityTypeBuilder<PaymentHistory> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.StripeSessionId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.StripePaymentIntentId)
            .HasMaxLength(255);

        builder.Property(p => p.Amount)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.StripeSessionId)
            .IsUnique();
    }
}
