using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.PaymentMethod;

public class EditModel : PageModel
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IPaymentMethodService paymentMethodService, ILogger<EditModel> logger)
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
            _logger.LogError(ex, "Error loading payment method for edit: {PaymentMethodId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the payment method.";
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
            var existingPaymentMethod = await _paymentMethodService.GetByIdAsync(PaymentMethod.Id);

            if (existingPaymentMethod == null)
            {
                return NotFound();
            }

            existingPaymentMethod.Name = PaymentMethod.Name;
            existingPaymentMethod.Description = PaymentMethod.Description;
            existingPaymentMethod.IsActive = PaymentMethod.IsActive;
            existingPaymentMethod.ModifiedBy = User.Identity?.Name ?? "System";
            existingPaymentMethod.ModifiedDate = DateTime.UtcNow;

            await _paymentMethodService.UpdateAsync(existingPaymentMethod);

            TempData["SuccessMessage"] = $"Payment method '{PaymentMethod.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment method: {PaymentMethodId}", PaymentMethod.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the payment method.");
            return Page();
        }
    }
}
