using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.Invoice;

public class CreateModel : PageModel
{
    private readonly IInvoiceService _invoiceService;
    private readonly IPersonService _personService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IInvoiceService invoiceService, IPersonService personService, IPaymentMethodService paymentMethodService, ILogger<CreateModel> logger)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InvoiceViewModel Invoice { get; set; } = new InvoiceViewModel { IsActive = true, InvoiceDate = DateTime.Now };

    public SelectList Persons { get; set; } = new SelectList(new List<Domain.Entities.Person>(), "Id", "FirstName");
    public SelectList PaymentMethods { get; set; } = new SelectList(new List<Domain.Entities.PaymentMethod>(), "Id", "Name");

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDropdownsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        try
        {
            var invoice = new Domain.Entities.Invoice
            {
                InvoiceNumber = Invoice.InvoiceNumber,
                PersonId = Invoice.PersonId,
                PaymentMethodId = Invoice.PaymentMethodId,
                InvoiceDate = Invoice.InvoiceDate,
                TotalAmount = Invoice.TotalAmount,
                IsActive = Invoice.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _invoiceService.CreateAsync(invoice);

            TempData["SuccessMessage"] = $"Invoice '{Invoice.InvoiceNumber}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice: {InvoiceNumber}", Invoice.InvoiceNumber);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the invoice.");
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var persons = await _personService.GetAllAsync();
        Persons = new SelectList(
            persons.Where(p => p.IsActive).OrderBy(p => p.FirstName),
            "Id",
            "FirstName",
            null,
            "LastName");

        var paymentMethods = await _paymentMethodService.GetAllAsync();
        PaymentMethods = new SelectList(paymentMethods.Where(pm => pm.IsActive).OrderBy(pm => pm.Name), "Id", "Name");
    }
}
