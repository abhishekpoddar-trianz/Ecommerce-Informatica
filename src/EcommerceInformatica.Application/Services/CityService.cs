using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class CityService : BaseService<City, CityService>, ICityService
{
    public CityService(IUnitOfWork unitOfWork, ILogger<CityService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<City> Repository => UnitOfWork.Cities;

    public async Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting city by name: {Name}", name);
            return await UnitOfWork.Cities.GetByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting city by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<City>> GetByProvinceIdAsync(int provinceId, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting cities for province ID: {ProvinceId}", provinceId);
            return await UnitOfWork.Cities.GetByProvinceIdAsync(provinceId, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting cities for province ID: {ProvinceId}", provinceId);
            throw;
        }
    }

    public async Task<IEnumerable<City>> GetActiveCitiesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting active cities");
            return await UnitOfWork.Cities.GetActiveCitiesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active cities");
            throw;
        }
    }
}
