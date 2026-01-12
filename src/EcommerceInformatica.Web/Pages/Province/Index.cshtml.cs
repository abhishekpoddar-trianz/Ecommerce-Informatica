using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Province;

public class IndexModel : PageModel
{
    private readonly IProvinceService _provinceService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IProvinceService provinceService, ILogger<IndexModel> logger)
    {
        _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<ProvinceViewModel> Provinces { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var provinces = await _provinceService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                provinces = provinces.Where(p =>
                    p.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase));
            }

            Provinces = provinces.Select(p => new ProvinceViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                CreatedBy = p.CreatedBy,
                ModifiedBy = p.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading provinces");
            TempData["ErrorMessage"] = "An error occurred while loading provinces.";
            Provinces = new List<ProvinceViewModel>();
        }
    }
}
