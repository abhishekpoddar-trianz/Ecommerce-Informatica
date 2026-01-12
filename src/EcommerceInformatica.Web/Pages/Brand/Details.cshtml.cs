using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Brand;

public class DetailsModel : PageModel
{
    private readonly IBrandService _brandService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IBrandService brandService, ILogger<DetailsModel> logger)
    {
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public BrandViewModel? Brand { get; set; }

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
            _logger.LogError(ex, "Error loading brand details for ID: {BrandId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading brand details.";
            return RedirectToPage("./Index");
        }
    }
}
