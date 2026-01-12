using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Category;

public class IndexModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ICategoryService categoryService, ILogger<IndexModel> logger)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<CategoryViewModel> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                categories = categories.Where(c =>
                    c.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (c.Description != null && c.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Categories = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                CreatedBy = c.CreatedBy,
                ModifiedBy = c.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading categories");
            TempData["ErrorMessage"] = "An error occurred while loading categories.";
            Categories = new List<CategoryViewModel>();
        }
    }
}
