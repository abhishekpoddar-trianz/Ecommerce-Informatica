using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceInformatica.Web.Pages.Product;

public class CreateModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        IProductService productService,
        ISupplierService supplierService,
        IBrandService brandService,
        ICategoryService categoryService,
        ILogger<CreateModel> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public ProductViewModel Product { get; set; } = new ProductViewModel { IsActive = true };

    public SelectList Suppliers { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList Brands { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList Categories { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create product page");
            TempData["ErrorMessage"] = "An error occurred while loading the form.";
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
            // Check if article ID already exists
            if (await _productService.ArticleIdExistsAsync(Product.ArticleId))
            {
                ModelState.AddModelError("Product.ArticleId", "A product with this Article ID already exists.");
                await LoadDropdownsAsync();
                return Page();
            }

            // Map ViewModel to Entity
            var product = new Domain.Entities.Product
            {
                ArticleId = Product.ArticleId,
                SupplierId = Product.SupplierId,
                BrandId = Product.BrandId,
                CategoryId = Product.CategoryId,
                Name = Product.Name,
                Stock = Product.Stock,
                UnitPrice = Product.UnitPrice,
                IsActive = Product.IsActive,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _productService.CreateAsync(product);

            TempData["SuccessMessage"] = $"Product '{Product.Name}' created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product: {ProductName}", Product.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
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
