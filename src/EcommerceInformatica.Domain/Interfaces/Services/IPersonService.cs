using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Person business logic operations
/// </summary>
public interface IPersonService
{
    Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Person?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Person>> GetByCityIdAsync(int cityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Person>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<Person> CreateAsync(Person person, CancellationToken cancellationToken = default);
    Task UpdateAsync(Person person, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken = default);
}
