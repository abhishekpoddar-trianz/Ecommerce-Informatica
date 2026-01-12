using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Product business logic operations
/// </summary>
public interface IProductService
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByBrandIdAsync(int brandId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Product?> GetByArticleIdAsync(string articleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsWithLowStockAsync(int threshold, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ArticleIdExistsAsync(string articleId, CancellationToken cancellationToken = default);
}
