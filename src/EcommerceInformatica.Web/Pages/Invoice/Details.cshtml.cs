using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Invoice;

public class DetailsModel : PageModel
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IInvoiceService invoiceService, ILogger<DetailsModel> logger)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public InvoiceViewModel? Invoice { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var invoice = await _invoiceService.GetByIdAsync(id.Value);

            if (invoice == null)
            {
                return NotFound();
            }

            Invoice = new InvoiceViewModel
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                PersonId = invoice.PersonId,
                PersonName = invoice.Person != null ? $"{invoice.Person.FirstName} {invoice.Person.LastName}" : null,
                PaymentMethodId = invoice.PaymentMethodId,
                PaymentMethodName = invoice.PaymentMethod?.Name,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                IsActive = invoice.IsActive,
                CreatedDate = invoice.CreatedDate,
                ModifiedDate = invoice.ModifiedDate,
                CreatedBy = invoice.CreatedBy,
                ModifiedBy = invoice.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoice details for ID: {InvoiceId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading invoice details.";
            return RedirectToPage("./Index");
        }
    }
}
