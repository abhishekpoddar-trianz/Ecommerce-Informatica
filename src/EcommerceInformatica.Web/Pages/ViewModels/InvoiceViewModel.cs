using System.ComponentModel.DataAnnotations;

namespace EcommerceInformatica.Web.Pages.ViewModels;

public class InvoiceViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Invoice number is required")]
    [Display(Name = "Invoice Number")]
    [StringLength(50, ErrorMessage = "Invoice number cannot exceed 50 characters")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Person is required")]
    [Display(Name = "Person")]
    public int PersonId { get; set; }

    [Display(Name = "Person Name")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Payment method is required")]
    [Display(Name = "Payment Method")]
    public int PaymentMethodId { get; set; }

    [Display(Name = "Payment Method")]
    public string? PaymentMethodName { get; set; }

    [Required(ErrorMessage = "Invoice date is required")]
    [Display(Name = "Invoice Date")]
    [DataType(DataType.Date)]
    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Total amount is required")]
    [Display(Name = "Total Amount")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
    [DataType(DataType.Currency)]
    public decimal TotalAmount { get; set; }

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
