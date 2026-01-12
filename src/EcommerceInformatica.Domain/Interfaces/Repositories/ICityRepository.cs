using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for City entity operations
/// </summary>
public interface ICityRepository : IRepository<City>
{
    Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<City>> GetByProvinceIdAsync(int provinceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<City>> GetActiveCitiesAsync(CancellationToken cancellationToken = default);
}
