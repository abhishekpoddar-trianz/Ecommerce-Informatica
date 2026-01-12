using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Category;

public class CreateModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ICategoryService categoryService, ILogger<CreateModel> logger)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CategoryViewModel Category { get; set; } = new CategoryViewModel { IsActive = true };

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
            var category = new Domain.Entities.Category
            {
                Name = Category.Name,
                Description = Category.Description,
                IsActive = Category.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _categoryService.CreateAsync(category);

            TempData["SuccessMessage"] = $"Category '{Category.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category: {CategoryName}", Category.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
            return Page();
        }
    }
}
