using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Category;

public class EditModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ICategoryService categoryService, ILogger<EditModel> logger)
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
            _logger.LogError(ex, "Error loading category for edit: {CategoryId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the category.";
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
            var existingCategory = await _categoryService.GetByIdAsync(Category.Id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = Category.Name;
            existingCategory.Description = Category.Description;
            existingCategory.IsActive = Category.IsActive;
            existingCategory.ModifiedBy = User.Identity?.Name ?? "System";
            existingCategory.ModifiedDate = DateTime.UtcNow;

            await _categoryService.UpdateAsync(existingCategory);

            TempData["SuccessMessage"] = $"Category '{Category.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category: {CategoryId}", Category.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
            return Page();
        }
    }
}
