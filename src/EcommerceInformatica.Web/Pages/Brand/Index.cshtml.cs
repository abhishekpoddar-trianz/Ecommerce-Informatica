using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Brand;

public class IndexModel : PageModel
{
    private readonly IBrandService _brandService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IBrandService brandService, ILogger<IndexModel> logger)
    {
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<BrandViewModel> Brands { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var brands = await _brandService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                brands = brands.Where(b =>
                    b.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.Description != null && b.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Brands = brands.Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                IsActive = b.IsActive,
                CreatedDate = b.CreatedDate,
                ModifiedDate = b.ModifiedDate,
                CreatedBy = b.CreatedBy,
                ModifiedBy = b.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading brands");
            TempData["ErrorMessage"] = "An error occurred while loading brands.";
            Brands = new List<BrandViewModel>();
        }
    }
}
