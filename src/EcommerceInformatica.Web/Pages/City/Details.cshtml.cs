using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.City;

public class DetailsModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ICityService cityService, ILogger<DetailsModel> logger)
    {
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public CityViewModel? City { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var city = await _cityService.GetByIdAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            City = new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                ProvinceId = city.ProvinceId,
                ProvinceName = city.Province?.Name,
                IsActive = city.IsActive,
                CreatedDate = city.CreatedDate,
                ModifiedDate = city.ModifiedDate,
                CreatedBy = city.CreatedBy,
                ModifiedBy = city.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading city details for ID: {CityId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading city details.";
            return RedirectToPage("./Index");
        }
    }
}
