using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

/// <summary>
/// Service implementation for Person business logic
/// </summary>
public class PersonService : IPersonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IUnitOfWork unitOfWork, ILogger<PersonService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting person with ID: {PersonId}", id);
            return await _unitOfWork.People.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person with ID: {PersonId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all people");
            return await _unitOfWork.People.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all people");
            throw;
        }
    }

    public async Task<Person?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting person with username: {Username}", username);
            return await _unitOfWork.People.GetByUsernameAsync(username, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person with username: {Username}", username);
            throw;
        }
    }

    public async Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting person with email: {Email}", email);
            return await _unitOfWork.People.GetByEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person with email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<Person>> GetByCityIdAsync(int cityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting people for city ID: {CityId}", cityId);
            return await _unitOfWork.People.GetByCityIdAsync(cityId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting people for city ID: {CityId}", cityId);
            throw;
        }
    }

    public async Task<IEnumerable<Person>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting active customers");
            return await _unitOfWork.People.GetActiveCustomersAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active customers");
            throw;
        }
    }

    public async Task<Person> CreateAsync(Person person, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new person: {Username}", person.Username);

            // Business validation
            if (await UsernameExistsAsync(person.Username, cancellationToken))
            {
                throw new InvalidOperationException($"Username '{person.Username}' already exists");
            }

            if (await EmailExistsAsync(person.Email, cancellationToken))
            {
                throw new InvalidOperationException($"Email '{person.Email}' already exists");
            }

            person.CreatedDate = DateTime.UtcNow;
            var createdPerson = await _unitOfWork.People.AddAsync(person, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Person created successfully with ID: {PersonId}", createdPerson.Id);
            return createdPerson;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating person: {Username}", person.Username);
            throw;
        }
    }

    public async Task UpdateAsync(Person person, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating person with ID: {PersonId}", person.Id);

            var existingPerson = await _unitOfWork.People.GetByIdAsync(person.Id, cancellationToken);
            if (existingPerson == null)
            {
                throw new KeyNotFoundException($"Person with ID {person.Id} not found");
            }

            person.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.People.UpdateAsync(person, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Person updated successfully with ID: {PersonId}", person.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating person with ID: {PersonId}", person.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting person with ID: {PersonId}", id);

            var person = await _unitOfWork.People.GetByIdAsync(id, cancellationToken);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {id} not found");
            }

            await _unitOfWork.People.DeleteAsync(person, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Person deleted successfully with ID: {PersonId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting person with ID: {PersonId}", id);
            throw;
        }
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.People.UsernameExistsAsync(username, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if username exists: {Username}", username);
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.People.EmailExistsAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if email exists: {Email}", email);
            throw;
        }
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating credentials for username: {Username}", username);

            var person = await _unitOfWork.People.GetByUsernameAsync(username, cancellationToken);
            if (person == null || !person.IsActive)
            {
                return false;
            }

            // TODO: Implement proper password hashing validation (BCrypt, etc.)
            // This is a placeholder - in production, use BCrypt.Net.BCrypt.Verify(password, person.PasswordHash)
            return person.PasswordHash == password;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating credentials for username: {Username}", username);
            throw;
        }
    }
}
