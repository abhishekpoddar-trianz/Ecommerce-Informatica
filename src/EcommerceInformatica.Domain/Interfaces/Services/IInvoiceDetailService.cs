using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Services;

/// <summary>
/// Service interface for InvoiceDetail business logic operations
/// </summary>
public interface IInvoiceDetailService
{
    Task<InvoiceDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceDetail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceDetail>> GetByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceDetail>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceDetail>> GetTopSellingProductsAsync(int count, CancellationToken cancellationToken = default);
    Task<InvoiceDetail> CreateAsync(InvoiceDetail invoiceDetail, CancellationToken cancellationToken = default);
    Task UpdateAsync(InvoiceDetail invoiceDetail, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
