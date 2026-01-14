using EcommerceInformatica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceInformatica.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.Property(i => i.InvoiceDate)
            .IsRequired();

        builder.Property(i => i.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(i => i.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(i => i.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(i => i.ModifiedDate)
            .IsRequired(false);

        builder.Property(i => i.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.ModifiedBy)
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(i => i.Person)
            .WithMany(p => p.Invoices)
            .HasForeignKey(i => i.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.PaymentMethod)
            .WithMany(pm => pm.Invoices)
            .HasForeignKey(i => i.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
