using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Supplier;

public class DeleteModel : PageModel
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ISupplierService supplierService, ILogger<DeleteModel> logger)
    {
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public SupplierViewModel Supplier { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var supplier = await _supplierService.GetByIdAsync(id.Value);

            if (supplier == null)
            {
                return NotFound();
            }

            Supplier = new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                IsActive = supplier.IsActive,
                CreatedDate = supplier.CreatedDate,
                ModifiedDate = supplier.ModifiedDate,
                CreatedBy = supplier.CreatedBy,
                ModifiedBy = supplier.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading supplier for delete: {SupplierId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the supplier.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _supplierService.DeleteAsync(Supplier.Id);

            TempData["SuccessMessage"] = $"Supplier '{Supplier.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting supplier: {SupplierId}", Supplier.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the supplier. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
