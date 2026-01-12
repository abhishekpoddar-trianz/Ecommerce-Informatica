using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class BrandService : BaseService<Brand, BrandService>, IBrandService
{
    public BrandService(IUnitOfWork unitOfWork, ILogger<BrandService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<Brand> Repository => UnitOfWork.Brands;

    public async Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting brand by name: {Name}", name);
            return await UnitOfWork.Brands.GetByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting brand by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting active brands");
            return await UnitOfWork.Brands.GetActiveBrandsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active brands");
            throw;
        }
    }

    public async Task<IEnumerable<Brand>> GetBrandsWithProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting brands with products");
            return await UnitOfWork.Brands.GetBrandsWithProductsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting brands with products");
            throw;
        }
    }
}
