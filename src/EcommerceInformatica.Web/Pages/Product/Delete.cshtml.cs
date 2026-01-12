using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Product;

public class DeleteModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IProductService productService, ILogger<DeleteModel> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public ProductViewModel Product { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var product = await _productService.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            Product = new ProductViewModel
            {
                Id = product.Id,
                ArticleId = product.ArticleId,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.Name,
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Name = product.Name,
                Stock = product.Stock,
                UnitPrice = product.UnitPrice,
                IsActive = product.IsActive,
                CreatedDate = product.CreatedDate,
                ModifiedDate = product.ModifiedDate,
                CreatedBy = product.CreatedBy,
                ModifiedBy = product.ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product for delete: {ProductId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the product.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _productService.DeleteAsync(Product.Id);

            TempData["SuccessMessage"] = $"Product '{Product.Name}' deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product: {ProductId}", Product.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the product. It may be in use by other records.";
            return RedirectToPage("./Index");
        }
    }
}
