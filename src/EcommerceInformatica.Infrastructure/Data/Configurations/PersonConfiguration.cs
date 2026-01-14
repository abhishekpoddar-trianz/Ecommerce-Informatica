using EcommerceInformatica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceInformatica.Infrastructure.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Email)
            .IsUnique();

        builder.Property(p => p.Phone)
            .HasMaxLength(20);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Username)
            .IsUnique();

        builder.Property(p => p.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.ModifiedDate)
            .IsRequired(false);

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(p => p.City)
            .WithMany(c => c.People)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
