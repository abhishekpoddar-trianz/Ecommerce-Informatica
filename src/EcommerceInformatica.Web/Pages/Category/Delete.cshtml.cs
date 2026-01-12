using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Category;

public class DeleteModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ICategoryService categoryService, ILogger<DeleteModel> logger)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CategoryViewModel Category { get; set; } = new();

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
            _logger.LogError(ex, "Error loading category for delete: {CategoryId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the category.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _categoryService.DeleteAsync(Category.Id);

            TempData["SuccessMessage"] = $"Category '{Category.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category: {CategoryId}", Category.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the category. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
