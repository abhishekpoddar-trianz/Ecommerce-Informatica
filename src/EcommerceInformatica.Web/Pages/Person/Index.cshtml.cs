using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Person;

public class IndexModel : PageModel
{
    private readonly IPersonService _personService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IPersonService personService, ILogger<IndexModel> logger)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<PersonViewModel> Persons { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var persons = await _personService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                persons = persons.Where(p =>
                    p.FirstName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    p.LastName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Username.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (p.Phone != null && p.Phone.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Persons = persons.Select(p => new PersonViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Phone = p.Phone,
                Address = p.Address,
                CityId = p.CityId,
                CityName = p.City?.Name,
                ProvinceName = p.City?.Province?.Name,
                Username = p.Username,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                CreatedBy = p.CreatedBy,
                ModifiedBy = p.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading persons");
            TempData["ErrorMessage"] = "An error occurred while loading persons.";
            Persons = new List<PersonViewModel>();
        }
    }
}
