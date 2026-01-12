using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class InvoiceDetailService : BaseService<InvoiceDetail, InvoiceDetailService>, IInvoiceDetailService
{
    public InvoiceDetailService(IUnitOfWork unitOfWork, ILogger<InvoiceDetailService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<InvoiceDetail> Repository => UnitOfWork.InvoiceDetails;

    public async Task<IEnumerable<InvoiceDetail>> GetByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoice details for invoice ID: {InvoiceId}", invoiceId);
            return await UnitOfWork.InvoiceDetails.GetByInvoiceIdAsync(invoiceId, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoice details for invoice ID: {InvoiceId}", invoiceId);
            throw;
        }
    }

    public async Task<IEnumerable<InvoiceDetail>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting invoice details for product ID: {ProductId}", productId);
            return await UnitOfWork.InvoiceDetails.GetByProductIdAsync(productId, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting invoice details for product ID: {ProductId}", productId);
            throw;
        }
    }

    public async Task<IEnumerable<InvoiceDetail>> GetTopSellingProductsAsync(int count, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting top {Count} selling products", count);
            return await UnitOfWork.InvoiceDetails.GetTopSellingProductsAsync(count, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting top selling products");
            throw;
        }
    }
}
