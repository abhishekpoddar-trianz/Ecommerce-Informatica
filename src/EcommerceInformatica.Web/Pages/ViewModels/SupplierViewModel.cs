using System.ComponentModel.DataAnnotations;

namespace EcommerceInformatica.Web.Pages.ViewModels;

public class SupplierViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Supplier name is required")]
    [Display(Name = "Name")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Contact Person")]
    [StringLength(200, ErrorMessage = "Contact person cannot exceed 200 characters")]
    public string? ContactPerson { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    public string? Email { get; set; }

    [Display(Name = "Phone")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string? Phone { get; set; }

    [Display(Name = "Address")]
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

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
