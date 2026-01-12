using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.City;

public class EditModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly IProvinceService _provinceService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ICityService cityService, IProvinceService provinceService, ILogger<EditModel> logger)
    {
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CityViewModel City { get; set; } = new();

    public SelectList Provinces { get; set; } = new SelectList(new List<Domain.Entities.Province>(), "Id", "Name");

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

            await LoadProvincesAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading city for edit: {CityId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the city.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadProvincesAsync();
            return Page();
        }

        try
        {
            var existingCity = await _cityService.GetByIdAsync(City.Id);

            if (existingCity == null)
            {
                return NotFound();
            }

            existingCity.Name = City.Name;
            existingCity.ProvinceId = City.ProvinceId;
            existingCity.IsActive = City.IsActive;
            existingCity.ModifiedBy = User.Identity?.Name ?? "System";
            existingCity.ModifiedDate = DateTime.UtcNow;

            await _cityService.UpdateAsync(existingCity);

            TempData["SuccessMessage"] = $"City '{City.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating city: {CityId}", City.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the city.");
            await LoadProvincesAsync();
            return Page();
        }
    }

    private async Task LoadProvincesAsync()
    {
        var provinces = await _provinceService.GetAllAsync();
        Provinces = new SelectList(provinces.Where(p => p.IsActive).OrderBy(p => p.Name), "Id", "Name");
    }
}
