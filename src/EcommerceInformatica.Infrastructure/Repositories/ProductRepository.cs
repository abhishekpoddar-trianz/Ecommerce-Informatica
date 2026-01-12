using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.SupplierId == supplierId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByBrandIdAsync(int brandId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.BrandId == brandId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByArticleIdAsync(string articleId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ArticleId == articleId, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsWithLowStockAsync(int threshold, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.Stock <= threshold && p.IsActive)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
