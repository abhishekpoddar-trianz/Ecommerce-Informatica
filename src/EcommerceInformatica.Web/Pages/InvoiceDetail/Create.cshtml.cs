using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.InvoiceDetail;

public class CreateModel : PageModel
{
    private readonly IInvoiceDetailService _invoiceDetailService;
    private readonly IInvoiceService _invoiceService;
    private readonly IProductService _productService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IInvoiceDetailService invoiceDetailService, IInvoiceService invoiceService, IProductService productService, ILogger<CreateModel> logger)
    {
        _invoiceDetailService = invoiceDetailService ?? throw new ArgumentNullException(nameof(invoiceDetailService));
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InvoiceDetailViewModel InvoiceDetail { get; set; } = new InvoiceDetailViewModel { IsActive = true };

    public SelectList Invoices { get; set; } = new SelectList(new List<Domain.Entities.Invoice>(), "Id", "InvoiceNumber");
    public SelectList Products { get; set; } = new SelectList(new List<Domain.Entities.Product>(), "Id", "Name");

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDropdownsAsync();
        return Page();
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
            var invoiceDetail = new Domain.Entities.InvoiceDetail
            {
                InvoiceId = InvoiceDetail.InvoiceId,
                ProductId = InvoiceDetail.ProductId,
                Quantity = InvoiceDetail.Quantity,
                UnitPrice = InvoiceDetail.UnitPrice,
                Subtotal = InvoiceDetail.Quantity * InvoiceDetail.UnitPrice,
                IsActive = InvoiceDetail.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _invoiceDetailService.CreateAsync(invoiceDetail);

            TempData["SuccessMessage"] = "Invoice detail created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice detail");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the invoice detail.");
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
