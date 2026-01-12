using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.InvoiceDetail;

public class DetailsModel : PageModel
{
    private readonly IInvoiceDetailService _invoiceDetailService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IInvoiceDetailService invoiceDetailService, ILogger<DetailsModel> logger)
    {
        _invoiceDetailService = invoiceDetailService ?? throw new ArgumentNullException(nameof(invoiceDetailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public InvoiceDetailViewModel? InvoiceDetail { get; set; }

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
            _logger.LogError(ex, "Error loading invoice detail details for ID: {InvoiceDetailId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading invoice detail details.";
            return RedirectToPage("./Index");
        }
    }
}
