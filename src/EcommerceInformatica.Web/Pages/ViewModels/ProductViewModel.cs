using System.ComponentModel.DataAnnotations;

namespace EcommerceInformatica.Web.Pages.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Article ID is required")]
    [Display(Name = "Article ID")]
    [StringLength(50, ErrorMessage = "Article ID cannot exceed 50 characters")]
    public string ArticleId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Supplier is required")]
    [Display(Name = "Supplier")]
    public int SupplierId { get; set; }

    [Display(Name = "Supplier Name")]
    public string? SupplierName { get; set; }

    [Required(ErrorMessage = "Brand is required")]
    [Display(Name = "Brand")]
    public int BrandId { get; set; }

    [Display(Name = "Brand Name")]
    public string? BrandName { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Category Name")]
    public string? CategoryName { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [Display(Name = "Product Name")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stock is required")]
    [Display(Name = "Stock")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Unit price is required")]
    [Display(Name = "Unit Price")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
    [DataType(DataType.Currency)]
    public decimal UnitPrice { get; set; }

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
