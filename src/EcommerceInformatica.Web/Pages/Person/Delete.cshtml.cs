using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Person;

public class DeleteModel : PageModel
{
    private readonly IPersonService _personService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IPersonService personService, ILogger<DeleteModel> logger)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public PersonViewModel Person { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var person = await _personService.GetByIdAsync(id.Value);

            if (person == null)
            {
                return NotFound();
            }

            Person = new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Phone = person.Phone,
                Address = person.Address,
                CityId = person.CityId,
                CityName = person.City?.Name,
                ProvinceName = person.City?.Province?.Name,
                Username = person.Username,
                IsActive = person.IsActive,
                CreatedDate = person.CreatedDate,
                ModifiedDate = person.ModifiedDate,
                CreatedBy = person.CreatedBy,
                ModifiedBy = person.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading person for delete: {PersonId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the person.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _personService.DeleteAsync(Person.Id);

            TempData["SuccessMessage"] = $"Person '{Person.FirstName} {Person.LastName}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting person: {PersonId}", Person.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the person. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
