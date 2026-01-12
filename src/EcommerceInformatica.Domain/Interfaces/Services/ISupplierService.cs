using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Supplier business logic operations
/// </summary>
public interface ISupplierService
{
    Task<Supplier?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> GetSuppliersWithProductsAsync(CancellationToken cancellationToken = default);
    Task<Supplier> CreateAsync(Supplier supplier, CancellationToken cancellationToken = default);
    Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
