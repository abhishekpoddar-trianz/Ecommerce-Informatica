using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class SupplierService : BaseService<Supplier, SupplierService>, ISupplierService
{
    public SupplierService(IUnitOfWork unitOfWork, ILogger<SupplierService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<Supplier> Repository => UnitOfWork.Suppliers;

    public async Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting supplier by name: {Name}", name);
            return await UnitOfWork.Suppliers.GetByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting supplier by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting active suppliers");
            return await UnitOfWork.Suppliers.GetActiveSuppliersAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active suppliers");
            throw;
        }
    }

    public async Task<IEnumerable<Supplier>> GetSuppliersWithProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting suppliers with products");
            return await UnitOfWork.Suppliers.GetSuppliersWithProductsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting suppliers with products");
            throw;
        }
    }
}
