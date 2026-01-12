using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.PaymentMethod;

public class CreateModel : PageModel
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IPaymentMethodService paymentMethodService, ILogger<CreateModel> logger)
    {
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public PaymentMethodViewModel PaymentMethod { get; set; } = new PaymentMethodViewModel { IsActive = true };

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
            var paymentMethod = new Domain.Entities.PaymentMethod
            {
                Name = PaymentMethod.Name,
                Description = PaymentMethod.Description,
                IsActive = PaymentMethod.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _paymentMethodService.CreateAsync(paymentMethod);

            TempData["SuccessMessage"] = $"Payment method '{PaymentMethod.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment method: {PaymentMethodName}", PaymentMethod.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the payment method.");
            return Page();
        }
    }
}
