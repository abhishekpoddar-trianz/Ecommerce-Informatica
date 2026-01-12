using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EcommerceInformatica.Application.Services;

public class PaymentMethodService : BaseService<PaymentMethod, PaymentMethodService>, IPaymentMethodService
{
    public PaymentMethodService(IUnitOfWork unitOfWork, ILogger<PaymentMethodService> logger)
        : base(unitOfWork, logger) { }

    protected override IRepository<PaymentMethod> Repository => UnitOfWork.PaymentMethods;

    public async Task<PaymentMethod?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting payment method by name: {Name}", name);
            return await UnitOfWork.PaymentMethods.GetByNameAsync(name, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting payment method by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Getting active payment methods");
            return await UnitOfWork.PaymentMethods.GetActivePaymentMethodsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active payment methods");
            throw;
        }
    }
}
