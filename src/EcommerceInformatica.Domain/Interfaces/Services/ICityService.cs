using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for City business logic operations
/// </summary>
public interface ICityService
{
    Task<City?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<City>> GetByProvinceIdAsync(int provinceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<City>> GetActiveCitiesAsync(CancellationToken cancellationToken = default);
    Task<City> CreateAsync(City city, CancellationToken cancellationToken = default);
    Task UpdateAsync(City city, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
