using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class ProvinceService : BaseService<Province, ProvinceService>, IProvinceService
{
    public ProvinceService(IUnitOfWork unitOfWork, ILogger<ProvinceService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<Province> Repository => UnitOfWork.Provinces;

    public async Task<Province?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting province by name: {Name}", name);
            return await UnitOfWork.Provinces.GetByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting province by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<Province>> GetActiveProvincesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting active provinces");
            return await UnitOfWork.Provinces.GetActiveProvincesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active provinces");
            throw;
        }
    }

    public async Task<IEnumerable<Province>> GetProvincesWithCitiesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting provinces with cities");
            return await UnitOfWork.Provinces.GetProvincesWithCitiesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting provinces with cities");
            throw;
        }
    }
}
