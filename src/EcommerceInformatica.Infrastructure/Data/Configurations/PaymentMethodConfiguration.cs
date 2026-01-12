using EcommerceInformatica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceInformatica.Infrastructure.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pm => pm.Description)
            .HasMaxLength(500);

        builder.Property(pm => pm.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(pm => pm.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(pm => pm.ModifiedDate)
            .IsRequired(false);

        builder.Property(pm => pm.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pm => pm.ModifiedBy)
            .HasMaxLength(100);
    }
}
