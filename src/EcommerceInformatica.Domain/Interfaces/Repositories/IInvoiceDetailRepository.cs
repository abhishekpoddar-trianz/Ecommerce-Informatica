using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for InvoiceDetail entity operations
/// </summary>
public interface IInvoiceDetailRepository : IRepository<InvoiceDetail>
{
    Task<IEnumerable<InvoiceDetail>> GetByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceDetail>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceDetail>> GetTopSellingProductsAsync(int count, CancellationToken cancellationToken = default);
}
