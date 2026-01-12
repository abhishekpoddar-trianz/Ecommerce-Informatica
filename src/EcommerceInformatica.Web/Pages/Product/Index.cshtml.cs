using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.Product;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IProductService productService, ILogger<IndexModel> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<ProductViewModel> Products { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var products = await _productService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                products = products.Where(p =>
                    p.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    p.ArticleId.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (p.Supplier != null && p.Supplier.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Brand != null && p.Brand.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Category != null && p.Category.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            Products = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                ArticleId = p.ArticleId,
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier?.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand?.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                Name = p.Name,
                Stock = p.Stock,
                UnitPrice = p.UnitPrice,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                CreatedBy = p.CreatedBy,
                ModifiedBy = p.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading products");
            TempData["ErrorMessage"] = "An error occurred while loading products.";
            Products = new List<ProductViewModel>();
        }
    }
}
