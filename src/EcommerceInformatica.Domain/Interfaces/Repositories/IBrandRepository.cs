using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Brand entity operations
/// </summary>
public interface IBrandRepository : IRepository<Brand>
{
    Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetBrandsWithProductsAsync(CancellationToken cancellationToken = default);
}
