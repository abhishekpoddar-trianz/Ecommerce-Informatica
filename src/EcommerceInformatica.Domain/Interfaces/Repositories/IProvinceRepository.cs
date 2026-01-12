using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Province entity operations
/// </summary>
public interface IProvinceRepository : IRepository<Province>
{
    Task<Province?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Province>> GetActiveProvincesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Province>> GetProvincesWithCitiesAsync(CancellationToken cancellationToken = default);
}
