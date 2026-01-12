using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Brand;

public class DeleteModel : PageModel
{
    private readonly IBrandService _brandService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IBrandService brandService, ILogger<DeleteModel> logger)
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
            _logger.LogError(ex, "Error loading brand for delete: {BrandId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the brand.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _brandService.DeleteAsync(Brand.Id);

            TempData["SuccessMessage"] = $"Brand '{Brand.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting brand: {BrandId}", Brand.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the brand. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
