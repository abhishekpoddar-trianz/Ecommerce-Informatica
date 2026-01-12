using EcommerceInformatica.Domain.Interfaces.Repositories;
using EcommerceInformatica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EcommerceInformatica.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing database transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IProductRepository? _products;
    private IPersonRepository? _people;
    private ISupplierRepository? _suppliers;
    private IBrandRepository? _brands;
    private ICategoryRepository? _categories;
    private IProvinceRepository? _provinces;
    private ICityRepository? _cities;
    private IInvoiceRepository? _invoices;
    private IInvoiceDetailRepository? _invoiceDetails;
    private IPaymentMethodRepository? _paymentMethods;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IProductRepository Products => _products ??= new ProductRepository(_context);
    public IPersonRepository People => _people ??= new PersonRepository(_context);
    public ISupplierRepository Suppliers => _suppliers ??= new SupplierRepository(_context);
    public IBrandRepository Brands => _brands ??= new BrandRepository(_context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    public IProvinceRepository Provinces => _provinces ??= new ProvinceRepository(_context);
    public ICityRepository Cities => _cities ??= new CityRepository(_context);
    public IInvoiceRepository Invoices => _invoices ??= new InvoiceRepository(_context);
    public IInvoiceDetailRepository InvoiceDetails => _invoiceDetails ??= new InvoiceDetailRepository(_context);
    public IPaymentMethodRepository PaymentMethods => _paymentMethods ??= new PaymentMethodRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
