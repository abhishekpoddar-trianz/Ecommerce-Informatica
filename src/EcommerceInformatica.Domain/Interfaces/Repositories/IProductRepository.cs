using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Product entity operations
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByBrandIdAsync(int brandId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Product?> GetByArticleIdAsync(string articleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsWithLowStockAsync(int threshold, CancellationToken cancellationToken = default);
}
