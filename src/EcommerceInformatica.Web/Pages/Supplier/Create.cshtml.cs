using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Supplier;

public class CreateModel : PageModel
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ISupplierService supplierService, ILogger<CreateModel> logger)
    {
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public SupplierViewModel Supplier { get; set; } = new SupplierViewModel { IsActive = true };

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var supplier = new Domain.Entities.Supplier
            {
                Name = Supplier.Name,
                ContactPerson = Supplier.ContactPerson,
                Email = Supplier.Email,
                Phone = Supplier.Phone,
                Address = Supplier.Address,
                IsActive = Supplier.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _supplierService.CreateAsync(supplier);

            TempData["SuccessMessage"] = $"Supplier '{Supplier.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating supplier: {SupplierName}", Supplier.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the supplier.");
            return Page();
        }
    }
}
