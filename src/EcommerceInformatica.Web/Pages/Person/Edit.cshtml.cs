using BCrypt.Net;
using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.Person;

public class EditModel : PageModel
{
    private readonly IPersonService _personService;
    private readonly ICityService _cityService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IPersonService personService, ICityService cityService, ILogger<EditModel> logger)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public PersonViewModel Person { get; set; } = new();

    public SelectList Cities { get; set; } = new SelectList(new List<Domain.Entities.City>(), "Id", "Name");

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

            await LoadCitiesAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading person for edit: {PersonId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the person.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Remove password validation if it's not provided (for edit)
        if (string.IsNullOrEmpty(Person.Password))
        {
            ModelState.Remove("Person.Password");
        }

        if (!ModelState.IsValid)
        {
            await LoadCitiesAsync();
            return Page();
        }

        try
        {
            var existingPerson = await _personService.GetByIdAsync(Person.Id);

            if (existingPerson == null)
            {
                return NotFound();
            }

            existingPerson.FirstName = Person.FirstName;
            existingPerson.LastName = Person.LastName;
            existingPerson.Email = Person.Email;
            existingPerson.Phone = Person.Phone;
            existingPerson.Address = Person.Address;
            existingPerson.CityId = Person.CityId;
            existingPerson.Username = Person.Username;

            // Only update password if a new one is provided
            if (!string.IsNullOrEmpty(Person.Password))
            {
                existingPerson.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Person.Password);
            }

            existingPerson.IsActive = Person.IsActive;
            existingPerson.ModifiedBy = User.Identity?.Name ?? "System";
            existingPerson.ModifiedDate = DateTime.UtcNow;

            await _personService.UpdateAsync(existingPerson);

            TempData["SuccessMessage"] = $"Person '{Person.FirstName} {Person.LastName}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating person: {PersonId}", Person.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the person.");
            await LoadCitiesAsync();
            return Page();
        }
    }

    private async Task LoadCitiesAsync()
    {
        var cities = await _cityService.GetAllAsync();
        Cities = new SelectList(cities.Where(c => c.IsActive).OrderBy(c => c.Name), "Id", "Name");
    }
}
