using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Supplier;

public class EditModel : PageModel
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ISupplierService supplierService, ILogger<EditModel> logger)
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
            _logger.LogError(ex, "Error loading supplier for edit: {SupplierId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the supplier.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var existingSupplier = await _supplierService.GetByIdAsync(Supplier.Id);

            if (existingSupplier == null)
            {
                return NotFound();
            }

            existingSupplier.Name = Supplier.Name;
            existingSupplier.ContactPerson = Supplier.ContactPerson;
            existingSupplier.Email = Supplier.Email;
            existingSupplier.Phone = Supplier.Phone;
            existingSupplier.Address = Supplier.Address;
            existingSupplier.IsActive = Supplier.IsActive;
            existingSupplier.ModifiedBy = User.Identity?.Name ?? "System";
            existingSupplier.ModifiedDate = DateTime.UtcNow;

            await _supplierService.UpdateAsync(existingSupplier);

            TempData["SuccessMessage"] = $"Supplier '{Supplier.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating supplier: {SupplierId}", Supplier.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the supplier.");
            return Page();
        }
    }
}
