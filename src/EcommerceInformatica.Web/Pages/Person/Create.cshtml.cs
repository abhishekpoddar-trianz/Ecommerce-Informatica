using BCrypt.Net;
using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.Person;

public class CreateModel : PageModel
{
    private readonly IPersonService _personService;
    private readonly ICityService _cityService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IPersonService personService, ICityService cityService, ILogger<CreateModel> logger)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public PersonViewModel Person { get; set; } = new PersonViewModel { IsActive = true };

    public SelectList Cities { get; set; } = new SelectList(new List<Domain.Entities.City>(), "Id", "Name");

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadCitiesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCitiesAsync();
            return Page();
        }

        try
        {
            var person = new Domain.Entities.Person
            {
                FirstName = Person.FirstName,
                LastName = Person.LastName,
                Email = Person.Email,
                Phone = Person.Phone,
                Address = Person.Address,
                CityId = Person.CityId,
                Username = Person.Username,
                PasswordHash = string.IsNullOrEmpty(Person.Password) ? string.Empty : BCrypt.Net.BCrypt.HashPassword(Person.Password),
                IsActive = Person.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _personService.CreateAsync(person);

            TempData["SuccessMessage"] = $"Person '{Person.FirstName} {Person.LastName}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating person: {PersonName}", $"{Person.FirstName} {Person.LastName}");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the person.");
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
