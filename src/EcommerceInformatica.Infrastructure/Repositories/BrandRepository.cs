using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class BrandRepository : Repository<Brand>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(b => b.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(b => b.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Brand>> GetBrandsWithProductsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(b => b.Products)
            .Where(b => b.Products.Any())
            .ToListAsync(cancellationToken);
    }
}
