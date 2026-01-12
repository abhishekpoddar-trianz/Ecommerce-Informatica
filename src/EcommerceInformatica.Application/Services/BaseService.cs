using EcommerceInformatica.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

/// <summary>
/// Base service class with common functionality
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TService">Service type</typeparam>
public abstract class BaseService<TEntity, TService> where TEntity : class
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger<TService> Logger;

    protected BaseService(IUnitOfWork unitOfWork, ILogger<TService> logger)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected abstract IRepository<TEntity> Repository { get; }

    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting {EntityType} with ID: {Id}", typeof(TEntity).Name, id);
            return await Repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting {EntityType} with ID: {Id}", typeof(TEntity).Name, id);
            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting all {EntityType}", typeof(TEntity).Name);
            return await Repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting all {EntityType}", typeof(TEntity).Name);
            throw;
        }
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Creating new {EntityType}", typeof(TEntity).Name);

            // Set audit fields via reflection
            SetAuditFields(entity, isCreating: true);

            var createdEntity = await Repository.AddAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("{EntityType} created successfully", typeof(TEntity).Name);
            return createdEntity;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating {EntityType}", typeof(TEntity).Name);
            throw;
        }
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Updating {EntityType}", typeof(TEntity).Name);

            // Set audit fields via reflection
            SetAuditFields(entity, isCreating: false);

            await Repository.UpdateAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("{EntityType} updated successfully", typeof(TEntity).Name);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating {EntityType}", typeof(TEntity).Name);
            throw;
        }
    }

    public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Deleting {EntityType} with ID: {Id}", typeof(TEntity).Name, id);

            var entity = await Repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity).Name} with ID {id} not found");
            }

            await Repository.DeleteAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("{EntityType} deleted successfully with ID: {Id}", typeof(TEntity).Name, id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting {EntityType} with ID: {Id}", typeof(TEntity).Name, id);
            throw;
        }
    }

    private void SetAuditFields(TEntity entity, bool isCreating)
    {
        var entityType = entity.GetType();

        if (isCreating)
        {
            var createdDateProp = entityType.GetProperty("CreatedDate");
            if (createdDateProp != null && createdDateProp.CanWrite)
            {
                createdDateProp.SetValue(entity, DateTime.UtcNow);
            }

            var createdByProp = entityType.GetProperty("CreatedBy");
            if (createdByProp != null && createdByProp.CanWrite)
            {
                // TODO: Get from current user context
                createdByProp.SetValue(entity, "System");
            }
        }
        else
        {
            var modifiedDateProp = entityType.GetProperty("ModifiedDate");
            if (modifiedDateProp != null && modifiedDateProp.CanWrite)
            {
                modifiedDateProp.SetValue(entity, DateTime.UtcNow);
            }

            var modifiedByProp = entityType.GetProperty("ModifiedBy");
            if (modifiedByProp != null && modifiedByProp.CanWrite)
            {
                // TODO: Get from current user context
                modifiedByProp.SetValue(entity, "System");
            }
        }
    }
}
