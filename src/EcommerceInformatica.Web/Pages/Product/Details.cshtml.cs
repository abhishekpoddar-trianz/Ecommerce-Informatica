using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Product;

public class DetailsModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IProductService productService, ILogger<DetailsModel> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ProductViewModel? Product { get; set; }

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
            _logger.LogError(ex, "Error loading product details for ID: {ProductId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading product details.";
            return RedirectToPage("./Index");
        }
    }
}
