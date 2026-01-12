using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

/// <summary>
/// Service implementation for Product business logic
/// </summary>
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", id);
            return await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product with ID: {ProductId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all products");
            return await _unitOfWork.Products.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting products for supplier ID: {SupplierId}", supplierId);
            return await _unitOfWork.Products.GetBySupplierIdAsync(supplierId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products for supplier ID: {SupplierId}", supplierId);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetByBrandIdAsync(int brandId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting products for brand ID: {BrandId}", brandId);
            return await _unitOfWork.Products.GetByBrandIdAsync(brandId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products for brand ID: {BrandId}", brandId);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting products for category ID: {CategoryId}", categoryId);
            return await _unitOfWork.Products.GetByCategoryIdAsync(categoryId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products for category ID: {CategoryId}", categoryId);
            throw;
        }
    }

    public async Task<Product?> GetByArticleIdAsync(string articleId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting product with article ID: {ArticleId}", articleId);
            return await _unitOfWork.Products.GetByArticleIdAsync(articleId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product with article ID: {ArticleId}", articleId);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting active products");
            return await _unitOfWork.Products.GetActiveProductsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active products");
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsWithLowStockAsync(int threshold, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting products with stock below threshold: {Threshold}", threshold);
            return await _unitOfWork.Products.GetProductsWithLowStockAsync(threshold, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products with low stock");
            throw;
        }
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new product: {ProductName}", product.Name);

            // Business validation
            if (await ArticleIdExistsAsync(product.ArticleId, cancellationToken))
            {
                throw new InvalidOperationException($"Product with Article ID '{product.ArticleId}' already exists");
            }

            product.CreatedDate = DateTime.UtcNow;
            var createdProduct = await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product created successfully with ID: {ProductId}", createdProduct.Id);
            return createdProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product: {ProductName}", product.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", product.Id);

            var existingProduct = await _unitOfWork.Products.GetByIdAsync(product.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found");
            }

            product.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product updated successfully with ID: {ProductId}", product.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID: {ProductId}", product.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);

            var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            await _unitOfWork.Products.DeleteAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product deleted successfully with ID: {ProductId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
            throw;
        }
    }

    public async Task<bool> ArticleIdExistsAsync(string articleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByArticleIdAsync(articleId, cancellationToken);
            return product != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if article ID exists: {ArticleId}", articleId);
            throw;
        }
    }
}
