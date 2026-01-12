using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.City;

public class IndexModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ICityService cityService, ILogger<IndexModel> logger)
    {
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<CityViewModel> Cities { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var cities = await _cityService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                cities = cities.Where(c =>
                    c.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (c.Province != null && c.Province.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Cities = cities.Select(c => new CityViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProvinceId = c.ProvinceId,
                ProvinceName = c.Province?.Name,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                CreatedBy = c.CreatedBy,
                ModifiedBy = c.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading cities");
            TempData["ErrorMessage"] = "An error occurred while loading cities.";
            Cities = new List<CityViewModel>();
        }
    }
}
