using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Person entity operations
/// </summary>
public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Person>> GetByCityIdAsync(int cityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Person>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
