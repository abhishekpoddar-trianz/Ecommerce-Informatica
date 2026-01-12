using System.ComponentModel.DataAnnotations;

namespace EcommerceInformatica.Web.Pages.ViewModels;

public class InvoiceDetailViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Invoice is required")]
    [Display(Name = "Invoice")]
    public int InvoiceId { get; set; }

    [Display(Name = "Invoice Number")]
    public string? InvoiceNumber { get; set; }

    [Required(ErrorMessage = "Product is required")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    [Display(Name = "Product Name")]
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Display(Name = "Quantity")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit price is required")]
    [Display(Name = "Unit Price")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
    [DataType(DataType.Currency)]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Subtotal")]
    [DataType(DataType.Currency)]
    public decimal Subtotal { get; set; }

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
