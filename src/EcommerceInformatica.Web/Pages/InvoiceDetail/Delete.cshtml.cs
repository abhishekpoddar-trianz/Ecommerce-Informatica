using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.InvoiceDetail;

public class DeleteModel : PageModel
{
    private readonly IInvoiceDetailService _invoiceDetailService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IInvoiceDetailService invoiceDetailService, ILogger<DeleteModel> logger)
    {
        _invoiceDetailService = invoiceDetailService ?? throw new ArgumentNullException(nameof(invoiceDetailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InvoiceDetailViewModel InvoiceDetail { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var invoiceDetail = await _invoiceDetailService.GetByIdAsync(id.Value);

            if (invoiceDetail == null)
            {
                return NotFound();
            }

            InvoiceDetail = new InvoiceDetailViewModel
            {
                Id = invoiceDetail.Id,
                InvoiceId = invoiceDetail.InvoiceId,
                InvoiceNumber = invoiceDetail.Invoice?.InvoiceNumber,
                ProductId = invoiceDetail.ProductId,
                ProductName = invoiceDetail.Product?.Name,
                Quantity = invoiceDetail.Quantity,
                UnitPrice = invoiceDetail.UnitPrice,
                Subtotal = invoiceDetail.Subtotal,
                IsActive = invoiceDetail.IsActive,
                CreatedDate = invoiceDetail.CreatedDate,
                ModifiedDate = invoiceDetail.ModifiedDate,
                CreatedBy = invoiceDetail.CreatedBy,
                ModifiedBy = invoiceDetail.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoice detail for delete: {InvoiceDetailId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the invoice detail.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _invoiceDetailService.DeleteAsync(InvoiceDetail.Id);

            TempData["SuccessMessage"] = "Invoice detail deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice detail: {InvoiceDetailId}", InvoiceDetail.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the invoice detail.";
            return RedirectToPage("./Index");
        }
    }
}
