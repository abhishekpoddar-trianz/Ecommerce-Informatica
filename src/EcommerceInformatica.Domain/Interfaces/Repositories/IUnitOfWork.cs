namespace EcommerceInformatica.Domain.Interfaces.Repositories;

/// <summary>
/// Unit of Work interface for managing database transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IPersonRepository People { get; }
    ISupplierRepository Suppliers { get; }
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }
    IProvinceRepository Provinces { get; }
    ICityRepository Cities { get; }
    IInvoiceRepository Invoices { get; }
    IInvoiceDetailRepository InvoiceDetails { get; }
    IPaymentMethodRepository PaymentMethods { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
