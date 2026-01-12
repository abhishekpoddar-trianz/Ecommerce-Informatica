using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Supplier;

public class IndexModel : PageModel
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ISupplierService supplierService, ILogger<IndexModel> logger)
    {
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<SupplierViewModel> Suppliers { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var suppliers = await _supplierService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                suppliers = suppliers.Where(s =>
                    s.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (s.ContactPerson != null && s.ContactPerson.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Email != null && s.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Phone != null && s.Phone.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Suppliers = suppliers.Select(s => new SupplierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                IsActive = s.IsActive,
                CreatedDate = s.CreatedDate,
                ModifiedDate = s.ModifiedDate,
                CreatedBy = s.CreatedBy,
                ModifiedBy = s.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading suppliers");
            TempData["ErrorMessage"] = "An error occurred while loading suppliers.";
            Suppliers = new List<SupplierViewModel>();
        }
    }
}
