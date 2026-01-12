using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Supplier;

public class DetailsModel : PageModel
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ISupplierService supplierService, ILogger<DetailsModel> logger)
    {
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public SupplierViewModel? Supplier { get; set; }

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
            _logger.LogError(ex, "Error loading supplier details for ID: {SupplierId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading supplier details.";
            return RedirectToPage("./Index");
        }
    }
}
