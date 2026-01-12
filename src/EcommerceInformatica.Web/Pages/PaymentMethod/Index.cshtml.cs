using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.PaymentMethod;

public class IndexModel : PageModel
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IPaymentMethodService paymentMethodService, ILogger<IndexModel> logger)
    {
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<PaymentMethodViewModel> PaymentMethods { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var paymentMethods = await _paymentMethodService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                paymentMethods = paymentMethods.Where(p =>
                    p.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (p.Description != null && p.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            PaymentMethods = paymentMethods.Select(p => new PaymentMethodViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                CreatedBy = p.CreatedBy,
                ModifiedBy = p.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment methods");
            TempData["ErrorMessage"] = "An error occurred while loading payment methods.";
            PaymentMethods = new List<PaymentMethodViewModel>();
        }
    }
}
