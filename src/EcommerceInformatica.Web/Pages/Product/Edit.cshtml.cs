using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.Product;

public class EditModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        IProductService productService,
        ISupplierService supplierService,
        IBrandService brandService,
        ICategoryService categoryService,
        ILogger<EditModel> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public ProductViewModel Product { get; set; } = new();

    public SelectList Suppliers { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList Brands { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList Categories { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

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
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Stock = product.Stock,
                UnitPrice = product.UnitPrice,
                IsActive = product.IsActive,
                CreatedDate = product.CreatedDate,
                ModifiedDate = product.ModifiedDate,
                CreatedBy = product.CreatedBy,
                ModifiedBy = product.ModifiedBy
            };

            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product for edit: {ProductId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the product.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        try
        {
            var existingProduct = await _productService.GetByIdAsync(Product.Id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Map ViewModel to Entity
            existingProduct.ArticleId = Product.ArticleId;
            existingProduct.SupplierId = Product.SupplierId;
            existingProduct.BrandId = Product.BrandId;
            existingProduct.CategoryId = Product.CategoryId;
            existingProduct.Name = Product.Name;
            existingProduct.Stock = Product.Stock;
            existingProduct.UnitPrice = Product.UnitPrice;
            existingProduct.IsActive = Product.IsActive;
            existingProduct.ModifiedBy = User.Identity?.Name ?? "System";
            existingProduct.ModifiedDate = DateTime.UtcNow;

            await _productService.UpdateAsync(existingProduct);

            TempData["SuccessMessage"] = $"Product '{Product.Name}' updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product: {ProductId}", Product.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var allSuppliers = await _supplierService.GetAllAsync();
        var suppliers = allSuppliers.Where(x => x.IsActive);

        var allBrands = await _brandService.GetAllAsync();
        var brands = allBrands.Where(x => x.IsActive);

        var allCategories = await _categoryService.GetAllAsync();
        var categories = allCategories.Where(x => x.IsActive);

        Suppliers = new SelectList(suppliers, "Id", "Name");
        Brands = new SelectList(brands, "Id", "Name");
        Categories = new SelectList(categories, "Id", "Name");
    }
}
