using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.Invoice;

public class EditModel : PageModel
{
    private readonly IInvoiceService _invoiceService;
    private readonly IPersonService _personService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IInvoiceService invoiceService, IPersonService personService, IPaymentMethodService paymentMethodService, ILogger<EditModel> logger)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InvoiceViewModel Invoice { get; set; } = new();

    public SelectList Persons { get; set; } = new SelectList(new List<Domain.Entities.Person>(), "Id", "FirstName");
    public SelectList PaymentMethods { get; set; } = new SelectList(new List<Domain.Entities.PaymentMethod>(), "Id", "Name");

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var invoice = await _invoiceService.GetByIdAsync(id.Value);

            if (invoice == null)
            {
                return NotFound();
            }

            Invoice = new InvoiceViewModel
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                PersonId = invoice.PersonId,
                PersonName = invoice.Person != null ? $"{invoice.Person.FirstName} {invoice.Person.LastName}" : null,
                PaymentMethodId = invoice.PaymentMethodId,
                PaymentMethodName = invoice.PaymentMethod?.Name,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                IsActive = invoice.IsActive,
                CreatedDate = invoice.CreatedDate,
                ModifiedDate = invoice.ModifiedDate,
                CreatedBy = invoice.CreatedBy,
                ModifiedBy = invoice.ModifiedBy
            };

            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoice for edit: {InvoiceId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the invoice.";
            return RedirectToPage("./Index");
        }
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
            var existingInvoice = await _invoiceService.GetByIdAsync(Invoice.Id);

            if (existingInvoice == null)
            {
                return NotFound();
            }

            existingInvoice.InvoiceNumber = Invoice.InvoiceNumber;
            existingInvoice.PersonId = Invoice.PersonId;
            existingInvoice.PaymentMethodId = Invoice.PaymentMethodId;
            existingInvoice.InvoiceDate = Invoice.InvoiceDate;
            existingInvoice.TotalAmount = Invoice.TotalAmount;
            existingInvoice.IsActive = Invoice.IsActive;
            existingInvoice.ModifiedBy = User.Identity?.Name ?? "System";
            existingInvoice.ModifiedDate = DateTime.UtcNow;

            await _invoiceService.UpdateAsync(existingInvoice);

            TempData["SuccessMessage"] = $"Invoice '{Invoice.InvoiceNumber}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice: {InvoiceId}", Invoice.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the invoice.");
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
