using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Province;

public class DeleteModel : PageModel
{
    private readonly IProvinceService _provinceService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IProvinceService provinceService, ILogger<DeleteModel> logger)
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
            _logger.LogError(ex, "Error loading province for delete: {ProvinceId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the province.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _provinceService.DeleteAsync(Province.Id);

            TempData["SuccessMessage"] = $"Province '{Province.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting province: {ProvinceId}", Province.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the province. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
