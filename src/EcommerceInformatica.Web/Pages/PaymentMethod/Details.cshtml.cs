using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.PaymentMethod;

public class DetailsModel : PageModel
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IPaymentMethodService paymentMethodService, ILogger<DetailsModel> logger)
    {
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public PaymentMethodViewModel? PaymentMethod { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var paymentMethod = await _paymentMethodService.GetByIdAsync(id.Value);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            PaymentMethod = new PaymentMethodViewModel
            {
                Id = paymentMethod.Id,
                Name = paymentMethod.Name,
                Description = paymentMethod.Description,
                IsActive = paymentMethod.IsActive,
                CreatedDate = paymentMethod.CreatedDate,
                ModifiedDate = paymentMethod.ModifiedDate,
                CreatedBy = paymentMethod.CreatedBy,
                ModifiedBy = paymentMethod.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment method details for ID: {PaymentMethodId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading payment method details.";
            return RedirectToPage("./Index");
        }
    }
}
