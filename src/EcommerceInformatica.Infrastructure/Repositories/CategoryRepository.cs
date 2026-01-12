using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(c => c.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Products)
            .Where(c => c.Products.Any())
            .ToListAsync(cancellationToken);
    }
}
