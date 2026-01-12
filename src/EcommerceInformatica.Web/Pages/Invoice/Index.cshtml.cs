using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Invoice;

public class IndexModel : PageModel
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IInvoiceService invoiceService, ILogger<IndexModel> logger)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<InvoiceViewModel> Invoices { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var invoices = await _invoiceService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                invoices = invoices.Where(i =>
                    i.InvoiceNumber.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (i.Person != null && (i.Person.FirstName + " " + i.Person.LastName).Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (i.PaymentMethod != null && i.PaymentMethod.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Invoices = invoices.Select(i => new InvoiceViewModel
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                PersonId = i.PersonId,
                PersonName = i.Person != null ? $"{i.Person.FirstName} {i.Person.LastName}" : null,
                PaymentMethodId = i.PaymentMethodId,
                PaymentMethodName = i.PaymentMethod?.Name,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount,
                IsActive = i.IsActive,
                CreatedDate = i.CreatedDate,
                ModifiedDate = i.ModifiedDate,
                CreatedBy = i.CreatedBy,
                ModifiedBy = i.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoices");
            TempData["ErrorMessage"] = "An error occurred while loading invoices.";
            Invoices = new List<InvoiceViewModel>();
        }
    }
}
