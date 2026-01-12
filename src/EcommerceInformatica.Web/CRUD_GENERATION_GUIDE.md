# CRUD Pages Generation Guide

This document provides templates and instructions for generating the remaining CRUD pages for all entities.

## Entities Needing Complete CRUD Pages

1. **Brand** - Simple entity (partially done, needs Details, Create, Edit, Delete)
2. **Category** - Simple entity
3. **Province** - Simple entity
4. **PaymentMethod** - Simple entity
5. **Supplier** - Simple entity with contact info
6. **City** - Entity with Province relationship
7. **Person** - Complex entity with City relationship
8. **Invoice** - Complex entity with Person and PaymentMethod relationships
9. **InvoiceDetail** - Complex entity with Invoice and Product relationships

## File Structure for Each Entity

For each entity, create 5 Razor Page pairs (10 files total):

```
Pages/[EntityName]/
├── Index.cshtml
├── Index.cshtml.cs
├── Details.cshtml
├── Details.cshtml.cs
├── Create.cshtml
├── Create.cshtml.cs
├── Edit.cshtml
├── Edit.cshtml.cs
├── Delete.cshtml
└── Delete.cshtml.cs
```

## Templates

### Simple Entity (Brand, Category, Province, PaymentMethod)

#### Index.cshtml Template
```razor
@page
@model EcommerceInformatica.Web.Pages.[Entity].IndexModel
@{
    ViewData["Title"] = "[Entities]";
}

<div class="container-fluid mt-4">
    <div class="row mb-3">
        <div class="col-md-6">
            <h2><i class="bi bi-[icon]"></i> [Entities]</h2>
        </div>
        <div class="col-md-6 text-end">
            <a asp-page="Create" class="btn btn-primary">
                <i class="bi bi-plus-circle"></i> Create New [Entity]
            </a>
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0"><i class="bi bi-search"></i> Search [Entities]</h5>
        </div>
        <div class="card-body">
            <form method="get">
                <div class="row">
                    <div class="col-md-10">
                        <input type="text" name="searchString" value="@Model.SearchString"
                               class="form-control" placeholder="Search by name..." />
                    </div>
                    <div class="col-md-2">
                        <button type="submit" class="btn btn-primary w-100">
                            <i class="bi bi-search"></i> Search
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <div class="card shadow-sm mt-3">
        <div class="card-body">
            @if (Model.[Entities].Any())
            {
                <div class="table-responsive">
                    <table class="table table-striped table-hover">
                        <thead class="table-primary">
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var item in Model.[Entities])
                            {
                                <tr>
                                    <td>@item.Name</td>
                                    <td>@item.Description</td>
                                    <td>
                                        @if (item.IsActive)
                                        {
                                            <span class="badge bg-success">Active</span>
                                        }
                                        else
                                        {
                                            <span class="badge bg-secondary">Inactive</span>
                                        }
                                    </td>
                                    <td>
                                        <div class="btn-group btn-group-sm" role="group">
                                            <a asp-page="Details" asp-route-id="@item.Id" class="btn btn-info" title="Details">
                                                <i class="bi bi-eye"></i>
                                            </a>
                                            <a asp-page="Edit" asp-route-id="@item.Id" class="btn btn-warning" title="Edit">
                                                <i class="bi bi-pencil"></i>
                                            </a>
                                            <a asp-page="Delete" asp-route-id="@item.Id" class="btn btn-danger" title="Delete">
                                                <i class="bi bi-trash"></i>
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                            }
                        </tbody>
                    </table>
                </div>
            }
            else
            {
                <div class="alert alert-info text-center" role="alert">
                    <i class="bi bi-info-circle"></i> No [entities] found.
                </div>
            }
        </div>
    </div>
</div>
```

#### Index.cshtml.cs Template
```csharp
using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.[Entity];

public class IndexModel : PageModel
{
    private readonly I[Entity]Service _[entity]Service;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(I[Entity]Service [entity]Service, ILogger<IndexModel> logger)
    {
        _[entity]Service = [entity]Service ?? throw new ArgumentNullException(nameof([entity]Service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<[Entity]ViewModel> [Entities] { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var [entities] = await _[entity]Service.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                [entities] = [entities].Where(x =>
                    x.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    (x.Description != null && x.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            [Entities] = [entities].Select(x => new [Entity]ViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                CreatedBy = x.CreatedBy,
                ModifiedBy = x.ModifiedBy
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading [entities]");
            TempData["ErrorMessage"] = "An error occurred while loading [entities].";
            [Entities] = new List<[Entity]ViewModel>();
        }
    }
}
```

### Details.cshtml Template
```razor
@page
@model EcommerceInformatica.Web.Pages.[Entity].DetailsModel
@{
    ViewData["Title"] = "[Entity] Details";
}

<div class="container mt-4">
    <div class="row mb-3">
        <div class="col-md-6">
            <h2><i class="bi bi-[icon]"></i> [Entity] Details</h2>
        </div>
        <div class="col-md-6 text-end">
            <a asp-page="Index" class="btn btn-secondary">
                <i class="bi bi-arrow-left"></i> Back to List
            </a>
        </div>
    </div>

    @if (Model.[Entity] != null)
    {
        <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
                <h5 class="mb-0">@Model.[Entity].Name</h5>
            </div>
            <div class="card-body">
                <dl class="row">
                    <dt class="col-sm-3">Name:</dt>
                    <dd class="col-sm-9">@Model.[Entity].Name</dd>

                    <dt class="col-sm-3">Description:</dt>
                    <dd class="col-sm-9">@Model.[Entity].Description</dd>

                    <dt class="col-sm-3">Status:</dt>
                    <dd class="col-sm-9">
                        @if (Model.[Entity].IsActive)
                        {
                            <span class="badge bg-success">Active</span>
                        }
                        else
                        {
                            <span class="badge bg-secondary">Inactive</span>
                        }
                    </dd>

                    <dt class="col-sm-3">Created Date:</dt>
                    <dd class="col-sm-9">@Model.[Entity].CreatedDate.ToString("MMM dd, yyyy HH:mm")</dd>

                    <dt class="col-sm-3">Created By:</dt>
                    <dd class="col-sm-9">@Model.[Entity].CreatedBy</dd>

                    @if (Model.[Entity].ModifiedDate.HasValue)
                    {
                        <dt class="col-sm-3">Modified Date:</dt>
                        <dd class="col-sm-9">@Model.[Entity].ModifiedDate.Value.ToString("MMM dd, yyyy HH:mm")</dd>

                        <dt class="col-sm-3">Modified By:</dt>
                        <dd class="col-sm-9">@Model.[Entity].ModifiedBy</dd>
                    }
                </dl>
            </div>
            <div class="card-footer">
                <div class="btn-group" role="group">
                    <a asp-page="Edit" asp-route-id="@Model.[Entity].Id" class="btn btn-warning">
                        <i class="bi bi-pencil"></i> Edit
                    </a>
                    <a asp-page="Delete" asp-route-id="@Model.[Entity].Id" class="btn btn-danger">
                        <i class="bi bi-trash"></i> Delete
                    </a>
                </div>
            </div>
        </div>
    }
    else
    {
        <div class="alert alert-warning" role="alert">
            <i class="bi bi-exclamation-triangle"></i> [Entity] not found.
        </div>
    }
</div>
```

### Details.cshtml.cs Template
```csharp
using EcommerceInformatica.Domain.Interfaces.Services;
using EcommerceInformatica.Web.Pages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceInformatica.Web.Pages.[Entity];

public class DetailsModel : PageModel
{
    private readonly I[Entity]Service _[entity]Service;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(I[Entity]Service [entity]Service, ILogger<DetailsModel> logger)
    {
        _[entity]Service = [entity]Service ?? throw new ArgumentNullException(nameof([entity]Service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public [Entity]ViewModel? [Entity] { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var [entity] = await _[entity]Service.GetByIdAsync(id.Value);

            if ([entity] == null)
            {
                return NotFound();
            }

            [Entity] = new [Entity]ViewModel
            {
                Id = [entity].Id,
                Name = [entity].Name,
                Description = [entity].Description,
                IsActive = [entity].IsActive,
                CreatedDate = [entity].CreatedDate,
                ModifiedDate = [entity].ModifiedDate,
                CreatedBy = [entity].CreatedBy,
                ModifiedBy = [entity].ModifiedBy
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading [entity] details for ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading [entity] details.";
            return RedirectToPage("./Index");
        }
    }
}
```

## Replacement Patterns

When using templates, replace:
- `[Entity]` → Entity name (e.g., `Brand`, `Category`)
- `[entity]` → Lowercase entity name (e.g., `brand`, `category`)
- `[Entities]` → Plural entity name (e.g., `Brands`, `Categories`)
- `[entities]` → Lowercase plural (e.g., `brands`, `categories`)
- `[icon]` → Bootstrap icon name (e.g., `award`, `grid`, `geo-alt`)

## Entity-Specific Considerations

### Province
- Very simple: Only Name and IsActive
- No Description field

### City
- Requires Province dropdown
- Include Province relationship in queries

### Person
- Include City dropdown
- Password field only on Create
- Username field

### Supplier
- ContactPerson, Email, Phone, Address fields
- No relationships

### Invoice
- Person and PaymentMethod dropdowns
- InvoiceNumber and InvoiceDate fields
- TotalAmount calculation

### InvoiceDetail
- Invoice and Product dropdowns
- Quantity, UnitPrice, Subtotal fields
- Auto-calculate Subtotal = Quantity * UnitPrice

## Next Steps

1. Use Product CRUD pages as the most complete reference
2. Follow templates above for simple entities
3. For complex entities (City, Person, Invoice, InvoiceDetail), add relationship handling
4. Ensure all services are properly injected in Program.cs
5. Test each CRUD operation thoroughly

## Additional Files Created

✅ Program.cs - Complete with all service registrations
✅ appsettings.json & appsettings.Development.json
✅ _ViewImports.cshtml
✅ _ViewStart.cshtml
✅ _Layout.cshtml - Bootstrap 5 with navigation
✅ Index.cshtml & Index.cshtml.cs - Home page with stats
✅ All ViewModels for 10 entities
✅ Product CRUD (complete - 10 files)
✅ Brand Index (2 files)
✅ Account pages (Login, Register, Logout)
✅ wwwroot/css/site.css
✅ wwwroot/js/site.js
✅ _ValidationScriptsPartial.cshtml

## Files Remaining to Generate

Use the templates above to generate the remaining CRUD pages for:
- Brand (Details, Create, Edit, Delete) - 8 files
- Category (all 5 pages) - 10 files
- Province (all 5 pages) - 10 files
- PaymentMethod (all 5 pages) - 10 files
- Supplier (all 5 pages) - 10 files
- City (all 5 pages) - 10 files
- Person (all 5 pages) - 10 files
- Invoice (all 5 pages) - 10 files
- InvoiceDetail (all 5 pages) - 10 files

**Total: 78 files remaining**
