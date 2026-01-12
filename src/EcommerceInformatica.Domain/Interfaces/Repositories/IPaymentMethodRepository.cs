using EcommerceInformatica.Domain.Entities;

namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for PaymentMethod entity operations
/// </summary>
public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    Task<PaymentMethod?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync(CancellationToken cancellationToken = default);
}
