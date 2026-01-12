using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Province;

public class DetailsModel : PageModel
{
    private readonly IProvinceService _provinceService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IProvinceService provinceService, ILogger<DetailsModel> logger)
    {
        _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ProvinceViewModel? Province { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var province = await _provinceService.GetByIdAsync(id.Value);

            if (province == null)
            {
                return NotFound();
            }

            Province = new ProvinceViewModel
            {
                Id = province.Id,
                Name = province.Name,
                IsActive = province.IsActive,
                CreatedDate = province.CreatedDate,
                ModifiedDate = province.ModifiedDate,
                CreatedBy = province.CreatedBy,
                ModifiedBy = province.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading province details for ID: {ProvinceId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading province details.";
            return RedirectToPage("./Index");
        }
    }
}
