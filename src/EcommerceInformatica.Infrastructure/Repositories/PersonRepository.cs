using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Person?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.City)
                .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(p => p.Username == username, cancellationToken);
    }

    public async Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.City)
                .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Person>> GetByCityIdAsync(int cityId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.City)
                .ThenInclude(c => c!.Province)
            .Where(p => p.CityId == cityId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Person>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.City)
                .ThenInclude(c => c!.Province)
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(p => p.Username == username, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(p => p.Email == email, cancellationToken);
    }

    public override async Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.City)
                .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
