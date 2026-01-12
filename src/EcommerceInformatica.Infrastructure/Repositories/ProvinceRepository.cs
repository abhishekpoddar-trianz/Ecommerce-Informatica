using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class ProvinceRepository : Repository<Province>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Province?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Province>> GetActiveProvincesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(p => p.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Province>> GetProvincesWithCitiesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Cities)
            .Where(p => p.Cities.Any())
            .ToListAsync(cancellationToken);
    }
}
