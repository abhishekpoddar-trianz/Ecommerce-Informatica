using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(i => i.Person)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(id => id.Product)
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(i => i.Person)
            .Include(i => i.PaymentMethod)
            .Where(i => i.PersonId == personId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetByPaymentMethodIdAsync(int paymentMethodId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(i => i.Person)
            .Include(i => i.PaymentMethod)
            .Where(i => i.PaymentMethodId == paymentMethodId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(i => i.Person)
            .Include(i => i.PaymentMethod)
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(i => i.Person)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(id => id.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate && i.IsActive)
            .SumAsync(i => i.TotalAmount, cancellationToken);
    }

    public override async Task<Invoice?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(i => i.Person)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceDetails)
                .ThenInclude(id => id.Product)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}
