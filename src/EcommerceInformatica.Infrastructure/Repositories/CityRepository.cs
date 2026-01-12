using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class CityRepository : Repository<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext context) : base(context) { }

    public async Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Province)
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<City>> GetByProvinceIdAsync(int provinceId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Province)
            .Where(c => c.ProvinceId == provinceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<City>> GetActiveCitiesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Province)
            .Where(c => c.IsActive)
            .ToListAsync(cancellationToken);
    }

    public override async Task<City?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Province)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
