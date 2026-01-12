using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Invoice entity operations
/// </summary>
public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetByPersonIdAsync(int personId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetByPaymentMethodIdAsync(int paymentMethodId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetInvoicesWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
