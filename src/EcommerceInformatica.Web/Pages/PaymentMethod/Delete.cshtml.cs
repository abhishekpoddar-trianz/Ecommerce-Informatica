using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.PaymentMethod;

public class DeleteModel : PageModel
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IPaymentMethodService paymentMethodService, ILogger<DeleteModel> logger)
    {
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public PaymentMethodViewModel PaymentMethod { get; set; } = new();

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
            _logger.LogError(ex, "Error loading payment method for delete: {PaymentMethodId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the payment method.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _paymentMethodService.DeleteAsync(PaymentMethod.Id);

            TempData["SuccessMessage"] = $"Payment method '{PaymentMethod.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment method: {PaymentMethodId}", PaymentMethod.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the payment method. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
