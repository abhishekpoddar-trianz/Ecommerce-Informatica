using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInformatica.Infrastructure.Repositories;

public class InvoiceDetailRepository : Repository<InvoiceDetail>, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<InvoiceDetail>> GetByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(id => id.Invoice)
            .Include(id => id.Product)
            .Where(id => id.InvoiceId == invoiceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InvoiceDetail>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(id => id.Invoice)
            .Include(id => id.Product)
            .Where(id => id.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InvoiceDetail>> GetTopSellingProductsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(id => id.Product)
            .GroupBy(id => id.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalQuantity = g.Sum(id => id.Quantity),
                Detail = g.First()
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(count)
            .Select(x => x.Detail)
            .ToListAsync(cancellationToken);
    }

    public override async Task<InvoiceDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(detail => detail.Invoice)
            .Include(detail => detail.Product)
            .FirstOrDefaultAsync(detail => detail.Id == id, cancellationToken);
    }
}
