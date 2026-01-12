using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.InvoiceDetail;

public class EditModel : PageModel
{
    private readonly IInvoiceDetailService _invoiceDetailService;
    private readonly IInvoiceService _invoiceService;
    private readonly IProductService _productService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IInvoiceDetailService invoiceDetailService, IInvoiceService invoiceService, IProductService productService, ILogger<EditModel> logger)
    {
        _invoiceDetailService = invoiceDetailService ?? throw new ArgumentNullException(nameof(invoiceDetailService));
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InvoiceDetailViewModel InvoiceDetail { get; set; } = new();

    public SelectList Invoices { get; set; } = new SelectList(new List<Domain.Entities.Invoice>(), "Id", "InvoiceNumber");
    public SelectList Products { get; set; } = new SelectList(new List<Domain.Entities.Product>(), "Id", "Name");

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

            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoice detail for edit: {InvoiceDetailId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the invoice detail.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        try
        {
            var existingInvoiceDetail = await _invoiceDetailService.GetByIdAsync(InvoiceDetail.Id);

            if (existingInvoiceDetail == null)
            {
                return NotFound();
            }

            existingInvoiceDetail.InvoiceId = InvoiceDetail.InvoiceId;
            existingInvoiceDetail.ProductId = InvoiceDetail.ProductId;
            existingInvoiceDetail.Quantity = InvoiceDetail.Quantity;
            existingInvoiceDetail.UnitPrice = InvoiceDetail.UnitPrice;
            existingInvoiceDetail.Subtotal = InvoiceDetail.Quantity * InvoiceDetail.UnitPrice;
            existingInvoiceDetail.IsActive = InvoiceDetail.IsActive;
            existingInvoiceDetail.ModifiedBy = User.Identity?.Name ?? "System";
            existingInvoiceDetail.ModifiedDate = DateTime.UtcNow;

            await _invoiceDetailService.UpdateAsync(existingInvoiceDetail);

            TempData["SuccessMessage"] = "Invoice detail updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice detail: {InvoiceDetailId}", InvoiceDetail.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the invoice detail.");
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var invoices = await _invoiceService.GetAllAsync();
        Invoices = new SelectList(invoices.Where(i => i.IsActive).OrderBy(i => i.InvoiceNumber), "Id", "InvoiceNumber");

        var products = await _productService.GetAllAsync();
        Products = new SelectList(products.Where(p => p.IsActive).OrderBy(p => p.Name), "Id", "Name");
    }
}
