using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Data;

/// <summary>
/// Application database context for EF Core
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceDetailConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());

        // Alternative: Apply all configurations from assembly
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    /// <summary>
    /// Override SaveChanges to add audit fields automatically
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Override SaveChanges to add audit fields automatically
    /// </summary>
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var createdDateProperty = entry.Entity.GetType().GetProperty("CreatedDate");
            var modifiedDateProperty = entry.Entity.GetType().GetProperty("ModifiedDate");
            var createdByProperty = entry.Entity.GetType().GetProperty("CreatedBy");
            var modifiedByProperty = entry.Entity.GetType().GetProperty("ModifiedBy");

            if (entry.State == EntityState.Added)
            {
                if (createdDateProperty != null && createdDateProperty.GetValue(entry.Entity) is DateTime createdDate && createdDate == default)
                {
                    createdDateProperty.SetValue(entry.Entity, DateTime.UtcNow);
                }

                if (createdByProperty != null && string.IsNullOrEmpty(createdByProperty.GetValue(entry.Entity) as string))
                {
                    // TODO: Get from current user context
                    createdByProperty.SetValue(entry.Entity, "System");
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (modifiedDateProperty != null)
                {
                    modifiedDateProperty.SetValue(entry.Entity, DateTime.UtcNow);
                }

                if (modifiedByProperty != null)
                {
                    // TODO: Get from current user context
                    modifiedByProperty.SetValue(entry.Entity, "System");
                }
            }
        }
    }
}
