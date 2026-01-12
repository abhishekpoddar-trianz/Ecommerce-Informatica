using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(s => s.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Supplier>> GetSuppliersWithProductsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Products)
            .Where(s => s.Products.Any())
            .ToListAsync(cancellationToken);
    }
}
