using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Supplier entity operations
/// </summary>
public interface ISupplierRepository : IRepository<Supplier>
{
    Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> GetSuppliersWithProductsAsync(CancellationToken cancellationToken = default);
}
