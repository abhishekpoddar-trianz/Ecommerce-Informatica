using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Brand;

public class EditModel : PageModel
{
    private readonly IBrandService _brandService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IBrandService brandService, ILogger<EditModel> logger)
    {
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public BrandViewModel Brand { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var brand = await _brandService.GetByIdAsync(id.Value);

            if (brand == null)
            {
                return NotFound();
            }

            Brand = new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                IsActive = brand.IsActive,
                CreatedDate = brand.CreatedDate,
                ModifiedDate = brand.ModifiedDate,
                CreatedBy = brand.CreatedBy,
                ModifiedBy = brand.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading brand for edit: {BrandId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the brand.";
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
            var existingBrand = await _brandService.GetByIdAsync(Brand.Id);

            if (existingBrand == null)
            {
                return NotFound();
            }

            existingBrand.Name = Brand.Name;
            existingBrand.Description = Brand.Description;
            existingBrand.IsActive = Brand.IsActive;
            existingBrand.ModifiedBy = User.Identity?.Name ?? "System";
            existingBrand.ModifiedDate = DateTime.UtcNow;

            await _brandService.UpdateAsync(existingBrand);

            TempData["SuccessMessage"] = $"Brand '{Brand.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating brand: {BrandId}", Brand.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the brand.");
            return Page();
        }
    }
}
