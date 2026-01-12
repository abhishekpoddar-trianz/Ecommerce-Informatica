using EcommerceInformatica.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;
    private readonly IInvoiceService _invoiceService;
    private readonly IPersonService _personService;
    private readonly ISupplierService _supplierService;
    private readonly ILogger<IndexModel> _logger;

    public int TotalProducts { get; set; }
    public int TotalInvoices { get; set; }
    public int TotalPersons { get; set; }
    public int TotalSuppliers { get; set; }

    public IndexModel(
        IProductService productService,
        IInvoiceService invoiceService,
        IPersonService personService,
        ISupplierService supplierService,
        ILogger<IndexModel> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task OnGetAsync()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            var invoices = await _invoiceService.GetAllAsync();
            var persons = await _personService.GetAllAsync();
            var suppliers = await _supplierService.GetAllAsync();

            TotalProducts = products.Count();
            TotalInvoices = invoices.Count();
            TotalPersons = persons.Count();
            TotalSuppliers = suppliers.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dashboard statistics");
            TotalProducts = 0;
            TotalInvoices = 0;
            TotalPersons = 0;
            TotalSuppliers = 0;
        }
    }
}
