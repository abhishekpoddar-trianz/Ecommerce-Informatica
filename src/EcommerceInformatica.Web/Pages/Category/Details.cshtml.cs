using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Category;

public class DetailsModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ICategoryService categoryService, ILogger<DetailsModel> logger)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public CategoryViewModel? Category { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var category = await _categoryService.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            Category = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedDate = category.CreatedDate,
                ModifiedDate = category.ModifiedDate,
                CreatedBy = category.CreatedBy,
                ModifiedBy = category.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading category details for ID: {CategoryId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading category details.";
            return RedirectToPage("./Index");
        }
    }
}
