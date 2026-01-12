using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Province;

public class EditModel : PageModel
{
    private readonly IProvinceService _provinceService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IProvinceService provinceService, ILogger<EditModel> logger)
    {
        _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public ProvinceViewModel Province { get; set; } = new();

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
            _logger.LogError(ex, "Error loading province for edit: {ProvinceId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the province.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var existingProvince = await _provinceService.GetByIdAsync(Province.Id);

            if (existingProvince == null)
            {
                return NotFound();
            }

            existingProvince.Name = Province.Name;
            existingProvince.IsActive = Province.IsActive;
            existingProvince.ModifiedBy = User.Identity?.Name ?? "System";
            existingProvince.ModifiedDate = DateTime.UtcNow;

            await _provinceService.UpdateAsync(existingProvince);

            TempData["SuccessMessage"] = $"Province '{Province.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating province: {ProvinceId}", Province.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the province.");
            return Page();
        }
    }
}
