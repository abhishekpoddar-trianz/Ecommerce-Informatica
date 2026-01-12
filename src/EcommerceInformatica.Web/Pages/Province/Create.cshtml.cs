using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Province;

public class CreateModel : PageModel
{
    private readonly IProvinceService _provinceService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IProvinceService provinceService, ILogger<CreateModel> logger)
    {
        _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public ProvinceViewModel Province { get; set; } = new ProvinceViewModel { IsActive = true };

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var province = new Domain.Entities.Province
            {
                Name = Province.Name,
                IsActive = Province.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _provinceService.CreateAsync(province);

            TempData["SuccessMessage"] = $"Province '{Province.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating province: {ProvinceName}", Province.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the province.");
            return Page();
        }
    }
}
