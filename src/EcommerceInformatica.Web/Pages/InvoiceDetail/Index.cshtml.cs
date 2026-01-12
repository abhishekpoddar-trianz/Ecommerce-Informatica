using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.InvoiceDetail;

public class IndexModel : PageModel
{
    private readonly IInvoiceDetailService _invoiceDetailService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IInvoiceDetailService invoiceDetailService, ILogger<IndexModel> logger)
    {
        _invoiceDetailService = invoiceDetailService ?? throw new ArgumentNullException(nameof(invoiceDetailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<InvoiceDetailViewModel> InvoiceDetails { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var invoiceDetails = await _invoiceDetailService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                invoiceDetails = invoiceDetails.Where(d =>
                    (d.Invoice != null && d.Invoice.InvoiceNumber.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (d.Product != null && d.Product.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            InvoiceDetails = invoiceDetails.Select(d => new InvoiceDetailViewModel
            {
                Id = d.Id,
                InvoiceId = d.InvoiceId,
                InvoiceNumber = d.Invoice?.InvoiceNumber,
                ProductId = d.ProductId,
                ProductName = d.Product?.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Subtotal = d.Subtotal,
                IsActive = d.IsActive,
                CreatedDate = d.CreatedDate,
                ModifiedDate = d.ModifiedDate,
                CreatedBy = d.CreatedBy,
                ModifiedBy = d.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoice details");
            TempData["ErrorMessage"] = "An error occurred while loading invoice details.";
            InvoiceDetails = new List<InvoiceDetailViewModel>();
        }
    }
}
