using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class InvoiceService : BaseService<Invoice, InvoiceService>, IInvoiceService
{
    public InvoiceService(IUnitOfWork unitOfWork, ILogger<InvoiceService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<Invoice> Repository => UnitOfWork.Invoices;

    public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoice by number: {InvoiceNumber}", invoiceNumber);
            return await UnitOfWork.Invoices.GetByInvoiceNumberAsync(invoiceNumber, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoice by number: {InvoiceNumber}", invoiceNumber);
            throw;
        }
    }

    public async Task<IEnumerable<Invoice>> GetByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoices for person ID: {PersonId}", personId);
            return await UnitOfWork.Invoices.GetByPersonIdAsync(personId, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoices for person ID: {PersonId}", personId);
            throw;
        }
    }

    public async Task<IEnumerable<Invoice>> GetByPaymentMethodIdAsync(int paymentMethodId, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoices for payment method ID: {PaymentMethodId}", paymentMethodId);
            return await UnitOfWork.Invoices.GetByPaymentMethodIdAsync(paymentMethodId, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoices for payment method ID: {PaymentMethodId}", paymentMethodId);
            throw;
        }
    }

    public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoices from {StartDate} to {EndDate}", startDate, endDate);
            return await UnitOfWork.Invoices.GetByDateRangeAsync(startDate, endDate, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoices by date range");
            throw;
        }
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoices with details");
            return await UnitOfWork.Invoices.GetInvoicesWithDetailsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoices with details");
            throw;
        }
    }

    public async Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting total sales from {StartDate} to {EndDate}", startDate, endDate);
            return await UnitOfWork.Invoices.GetTotalSalesByDateRangeAsync(startDate, endDate, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting total sales by date range");
            throw;
        }
    }

    public Task<string> GenerateInvoiceNumberAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Generating new invoice number");
            var timestamp = DateTime.UtcNow;
            var invoiceNumber = $"INV-{timestamp:yyyyMMdd}-{timestamp:HHmmss}-{Guid.NewGuid():N}".Substring(0, 30);
            return Task.FromResult(invoiceNumber);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error generating invoice number");
            throw;
        }
    }
}
