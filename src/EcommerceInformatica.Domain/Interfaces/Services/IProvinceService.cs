using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Province business logic operations
/// </summary>
public interface IProvinceService
{
    Task<Province?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Province>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Province?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Province>> GetActiveProvincesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Province>> GetProvincesWithCitiesAsync(CancellationToken cancellationToken = default);
    Task<Province> CreateAsync(Province province, CancellationToken cancellationToken = default);
    Task UpdateAsync(Province province, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
