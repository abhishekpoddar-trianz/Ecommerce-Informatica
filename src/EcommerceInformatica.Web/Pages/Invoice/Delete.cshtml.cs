using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Invoice;

public class DeleteModel : PageModel
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IInvoiceService invoiceService, ILogger<DeleteModel> logger)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InvoiceViewModel Invoice { get; set; } = new();

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
            _logger.LogError(ex, "Error loading invoice for delete: {InvoiceId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the invoice.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _invoiceService.DeleteAsync(Invoice.Id);

            TempData["SuccessMessage"] = $"Invoice '{Invoice.InvoiceNumber}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice: {InvoiceId}", Invoice.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the invoice. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
