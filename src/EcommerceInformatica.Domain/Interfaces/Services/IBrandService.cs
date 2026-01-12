using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Brand business logic operations
/// </summary>
public interface IBrandService
{
    Task<Brand?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetBrandsWithProductsAsync(CancellationToken cancellationToken = default);
    Task<Brand> CreateAsync(Brand brand, CancellationToken cancellationToken = default);
    Task UpdateAsync(Brand brand, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
