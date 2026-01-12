using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Brand;

public class CreateModel : PageModel
{
    private readonly IBrandService _brandService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IBrandService brandService, ILogger<CreateModel> logger)
    {
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public BrandViewModel Brand { get; set; } = new BrandViewModel { IsActive = true };

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
            var brand = new Domain.Entities.Brand
            {
                Name = Brand.Name,
                Description = Brand.Description,
                IsActive = Brand.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _brandService.CreateAsync(brand);

            TempData["SuccessMessage"] = $"Brand '{Brand.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating brand: {BrandName}", Brand.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the brand.");
            return Page();
        }
    }
}
