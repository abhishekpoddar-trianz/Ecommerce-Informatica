using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.City;

public class DeleteModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ICityService cityService, ILogger<DeleteModel> logger)
    {
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CityViewModel City { get; set; } = new();

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
            _logger.LogError(ex, "Error loading city for delete: {CityId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the city.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _cityService.DeleteAsync(City.Id);

            TempData["SuccessMessage"] = $"City '{City.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting city: {CityId}", City.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the city. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
