using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.City;

public class CreateModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly IProvinceService _provinceService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ICityService cityService, IProvinceService provinceService, ILogger<CreateModel> logger)
    {
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CityViewModel City { get; set; } = new CityViewModel { IsActive = true };

    public SelectList Provinces { get; set; } = new SelectList(new List<Domain.Entities.Province>(), "Id", "Name");

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadProvincesAsync();
        return Page();
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
            var city = new Domain.Entities.City
            {
                Name = City.Name,
                ProvinceId = City.ProvinceId,
                IsActive = City.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _cityService.CreateAsync(city);

            TempData["SuccessMessage"] = $"City '{City.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating city: {CityName}", City.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the city.");
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
