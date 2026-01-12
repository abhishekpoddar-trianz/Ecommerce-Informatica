using System.ComponentModel.DataAnnotations;

namespace EcommerceInformatica.Web.Pages.ViewModels;

public class CityViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "City name is required")]
    [Display(Name = "Name")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Province is required")]
    [Display(Name = "Province")]
    public int ProvinceId { get; set; }

    [Display(Name = "Province Name")]
    public string? ProvinceName { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Created Date")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Modified Date")]
    [DataType(DataType.DateTime)]
    public DateTime? ModifiedDate { get; set; }

    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "Modified By")]
    public string? ModifiedBy { get; set; }
}
