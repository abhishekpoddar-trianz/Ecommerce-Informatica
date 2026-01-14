using EcommerceInformatica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceInformatica.Infrastructure.Data.Configurations;

public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder.ToTable("InvoiceDetails");

        builder.HasKey(id => id.Id);

        builder.Property(id => id.Quantity)
            .IsRequired();

        builder.Property(id => id.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(id => id.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(id => id.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(id => id.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(id => id.ModifiedDate)
            .IsRequired(false);

        builder.Property(id => id.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(id => id.ModifiedBy)
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(id => id.Invoice)
            .WithMany(i => i.InvoiceDetails)
            .HasForeignKey(id => id.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(id => id.Product)
            .WithMany(p => p.InvoiceDetails)
            .HasForeignKey(id => id.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
