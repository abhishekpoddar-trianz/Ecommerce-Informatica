# Ecommerce Informatica - Web Layer

This is the ASP.NET Core 8 Razor Pages web application for the Ecommerce Informatica system, built following Clean Architecture principles.

## Project Structure

```
EcommerceInformatica.Web/
├── Pages/
│   ├── Shared/
│   │   ├── _Layout.cshtml              ✅ Main layout with Bootstrap 5
│   │   └── _ValidationScriptsPartial.cshtml  ✅ Validation scripts
│   ├── ViewModels/                     ✅ All 10 entity ViewModels
│   ├── Product/                        ✅ Complete CRUD (10 files)
│   ├── Brand/                          ✅ Complete CRUD (10 files)
│   ├── Category/                       ⚠️ To be generated
│   ├── Province/                       ⚠️ To be generated
│   ├── City/                          ⚠️ To be generated
│   ├── Supplier/                       ⚠️ To be generated
│   ├── Person/                         ⚠️ To be generated
│   ├── Invoice/                        ⚠️ To be generated
│   ├── InvoiceDetail/                  ⚠️ To be generated
│   ├── PaymentMethod/                  ⚠️ To be generated
│   ├── Account/                        ✅ Login, Register, Logout
│   ├── Index.cshtml                    ✅ Home page with dashboard
│   ├── Index.cshtml.cs                 ✅ Home page logic
│   ├── _ViewImports.cshtml             ✅ Global imports
│   └── _ViewStart.cshtml               ✅ Layout selection
├── wwwroot/
│   ├── css/
│   │   └── site.css                    ✅ Custom CSS with Bootstrap 5 theming
│   └── js/
│       └── site.js                     ✅ Custom JavaScript utilities
├── Extensions/                         (Pre-existing)
├── Program.cs                          ✅ ASP.NET Core 8 startup with Serilog
├── appsettings.json                    ✅ Production configuration
├── appsettings.Development.json        ✅ Development configuration
├── EcommerceInformatica.Web.csproj     ✅ Project file
├── CRUD_GENERATION_GUIDE.md            ✅ Templates for remaining pages
├── Generate-CRUDPages.ps1              ✅ PowerShell generation script
└── README.md                           ✅ This file
```

## Completed Components

### ✅ Core Configuration
- **Program.cs** - Complete ASP.NET Core 8 startup with:
  - Serilog logging (console + file)
  - SQL Server with Entity Framework Core
  - ASP.NET Core Identity for authentication
  - All service registrations (10 entity services)
  - Cookie authentication
  - Session support

- **appsettings.json** - Production configuration
- **appsettings.Development.json** - Development configuration with enhanced logging

### ✅ Shared Layout & Views
- **_Layout.cshtml** - Bootstrap 5 responsive layout with:
  - Navigation menu with dropdowns for Products, Sales, Administration
  - User authentication display (Login/Register or Username/Logout)
  - Alert messaging system (Success, Error, Info)
  - Mobile-responsive navbar

- **_ViewImports.cshtml** - Global namespaces and tag helpers
- **_ViewStart.cshtml** - Default layout selection
- **_ValidationScriptsPartial.cshtml** - jQuery validation scripts

### ✅ Home Page
- **Index.cshtml** - Dashboard with:
  - Quick stats (Products, Invoices, Persons, Suppliers)
  - Three-card feature navigation
  - System information panel

- **Index.cshtml.cs** - Loads statistics from services

### ✅ ViewModels (All 10 Entities)
Complete ViewModels with validation attributes:
1. **ProductViewModel** - Product with Supplier, Brand, Category relationships
2. **PersonViewModel** - Person with City relationship
3. **SupplierViewModel** - Supplier information
4. **BrandViewModel** - Brand information
5. **CategoryViewModel** - Category information
6. **ProvinceViewModel** - Province information
7. **CityViewModel** - City with Province relationship
8. **InvoiceViewModel** - Invoice with Person and PaymentMethod relationships
9. **InvoiceDetailViewModel** - Invoice detail with Invoice and Product relationships
10. **PaymentMethodViewModel** - Payment method information

### ✅ Complete CRUD Pages

#### Product (Complete - 10 files)
- **Index** - List with search, filterable by name/article ID/supplier/brand/category
- **Details** - Full product information display
- **Create** - Form with supplier/brand/category dropdowns
- **Edit** - Update form with audit info
- **Delete** - Confirmation page with full details

#### Brand (Complete - 10 files)
- **Index** - List with search by name/description
- **Details** - Brand information display
- **Create** - Simple form for brand creation
- **Edit** - Update form with audit info
- **Delete** - Confirmation page

### ✅ Account Pages
- **Login.cshtml** + **Login.cshtml.cs** - Login with email/password, remember me
- **Register.cshtml** + **Register.cshtml.cs** - Registration with password confirmation
- **Logout.cshtml.cs** - Logout handler

### ✅ Static Assets
- **site.css** - Custom CSS with:
  - Bootstrap 5 enhancements
  - Custom card, table, alert styles
  - Form validation styling
  - Dashboard stat animations
  - Responsive design utilities
  - Custom scrollbar

- **site.js** - JavaScript utilities:
  - Auto-hide alerts after 5 seconds
  - Delete confirmation dialogs
  - Form validation enhancements
  - Subtotal calculation for invoice details
  - Currency input formatting
  - Loading spinner utilities
  - Export to CSV functionality

## Remaining Work

### ⚠️ CRUD Pages to Generate (8 entities × 10 files = 80 files)

Use the templates in `CRUD_GENERATION_GUIDE.md` to generate:

1. **Category** (Simple entity like Brand)
2. **Province** (Simplest - only Name field)
3. **PaymentMethod** (Simple entity like Brand)
4. **Supplier** (Simple with contact fields)
5. **City** (With Province dropdown)
6. **Person** (Complex - with City dropdown, Username, Password)
7. **Invoice** (Complex - with Person and PaymentMethod dropdowns)
8. **InvoiceDetail** (Complex - with Invoice and Product dropdowns, subtotal calc)

### Generation Options

#### Option 1: Manual Creation (Recommended for learning)
Follow the templates in `CRUD_GENERATION_GUIDE.md` and use Product/Brand as reference examples.

#### Option 2: Script-Assisted
Run the provided PowerShell script:
```powershell
cd src/EcommerceInformatica.Web
.\Generate-CRUDPages.ps1
```
Note: This script generates only Index pages. Use it as a starting point.

#### Option 3: Copy & Modify
1. Copy the Brand folder
2. Rename files and classes
3. Update entity-specific fields
4. Adjust relationships (dropdowns for foreign keys)

## Key Features

### Architecture Patterns
- **Clean Architecture** - Web layer depends only on Application and Domain
- **Manual Mapping** - ViewModels ↔ DTOs mapping in PageModels (no AutoMapper in Web)
- **Dependency Injection** - All services injected via constructor
- **Async/Await** - All database operations are async
- **Error Handling** - Try-catch with logging and user-friendly messages
- **TempData** - Success/Error messages passed between pages

### Security
- **ASP.NET Core Identity** - User authentication and authorization
- **Password Requirements** - Min 6 chars, upper/lower/digit required
- **Account Lockout** - 5 failed attempts, 5-minute lockout
- **HTTPS** - Enforced via middleware
- **Anti-Forgery Tokens** - Automatic via Razor Pages

### Validation
- **Client-Side** - jQuery Validation Unobtrusive
- **Server-Side** - Data Annotations in ViewModels
- **Custom Validation** - Business rules in PageModels
- **User-Friendly Errors** - Displayed inline and in summary

### UI/UX
- **Bootstrap 5** - Modern, responsive design
- **Bootstrap Icons** - Consistent iconography
- **Color-Coded Badges** - Status, stock levels
- **Responsive Tables** - Scrollable on mobile
- **Alert System** - Success (green), Error (red), Info (blue), Warning (yellow)
- **Auto-Dismiss Alerts** - Fade out after 5 seconds
- **Confirmation Dialogs** - For delete operations

## Running the Application

### Prerequisites
- .NET 8 SDK
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code

### Steps

1. **Update Connection String**
   ```json
   // appsettings.json or appsettings.Development.json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EcommerceInformaticaDB;..."
   }
   ```

2. **Apply Migrations**
   ```bash
   dotnet ef database update --project ../EcommerceInformatica.Infrastructure
   ```

3. **Run Application**
   ```bash
   dotnet run
   ```

4. **Navigate to**
   ```
   https://localhost:5001
   ```

## Development Guidelines

### Creating New CRUD Pages

1. **Create ViewModel** in `Pages/ViewModels/`
   - Add validation attributes
   - Include all entity properties

2. **Create Razor Page Pairs**
   - Index (list with search)
   - Details (read-only display)
   - Create (form with validation)
   - Edit (form with audit info)
   - Delete (confirmation with details)

3. **Follow Naming Conventions**
   - Namespace: `EcommerceInformatica.Web.Pages.[Entity]`
   - Model class: `IndexModel`, `DetailsModel`, etc.
   - Properties: Pascal case
   - Methods: Async with `Async` suffix

4. **Implement Error Handling**
   ```csharp
   try
   {
       // Operation
       TempData["SuccessMessage"] = "Success!";
       return RedirectToPage("./Index");
   }
   catch (Exception ex)
   {
       _logger.LogError(ex, "Error message");
       TempData["ErrorMessage"] = "User-friendly error";
       return Page();
   }
   ```

5. **Use Dependency Injection**
   ```csharp
   private readonly IEntityService _entityService;
   private readonly ILogger<PageModel> _logger;

   public PageModel(IEntityService entityService, ILogger<PageModel> logger)
   {
       _entityService = entityService ?? throw new ArgumentNullException(...);
       _logger = logger ?? throw new ArgumentNullException(...);
   }
   ```

### Manual Mapping Pattern
```csharp
// Entity to ViewModel
var viewModel = new EntityViewModel
{
    Id = entity.Id,
    Name = entity.Name,
    // ... map all properties
};

// ViewModel to Entity (Create)
var entity = new Entity
{
    Name = viewModel.Name,
    CreatedBy = User.Identity?.Name ?? "System",
    CreatedDate = DateTime.UtcNow
};

// ViewModel to Entity (Update)
existingEntity.Name = viewModel.Name;
existingEntity.ModifiedBy = User.Identity?.Name ?? "System";
existingEntity.ModifiedDate = DateTime.UtcNow;
```

### Loading Dropdowns
```csharp
private async Task LoadDropdownsAsync()
{
    var items = await _relatedService.GetActiveAsync();
    Dropdown = new SelectList(items, "Id", "Name");
}
```

## Troubleshooting

### Common Issues

1. **"Service not registered"**
   - Check `Program.cs` for service registration
   - Verify interface and implementation names match

2. **"Invalid ModelState"**
   - Check ViewModel validation attributes
   - Ensure all required fields are provided

3. **"DbUpdateException"**
   - Check foreign key constraints
   - Verify related entities exist

4. **"Navigation property is null"**
   - Ensure service uses `.Include()` for related entities
   - Check repository implementation

5. **"404 Not Found"**
   - Verify page file naming (matches class name)
   - Check namespace matches folder structure
   - Ensure `@page` directive is at top of .cshtml

## Testing

### Manual Testing Checklist
- [ ] Home page loads with correct stats
- [ ] Navigation menu works
- [ ] Login/Register/Logout work
- [ ] CRUD operations for each entity:
  - [ ] List displays all items
  - [ ] Search filters correctly
  - [ ] Create saves new record
  - [ ] Details shows full information
  - [ ] Edit updates record
  - [ ] Delete removes record
- [ ] Validation messages display
- [ ] Alert messages display and auto-hide
- [ ] Responsive design on mobile

## Next Steps

1. **Generate Remaining CRUD Pages** - Use templates in `CRUD_GENERATION_GUIDE.md`
2. **Add Authorization** - Role-based access control
3. **Implement Search** - Advanced filtering and pagination
4. **Add Reports** - Invoice reports, inventory reports
5. **Enhance UI** - Charts, graphs, advanced components
6. **Performance** - Caching, pagination, lazy loading
7. **Testing** - Unit tests, integration tests
8. **Deployment** - Docker, Azure, IIS

## Technologies Used

- **ASP.NET Core 8** - Web framework
- **Razor Pages** - Page-based programming model
- **Entity Framework Core 8** - ORM
- **ASP.NET Core Identity** - Authentication/Authorization
- **Serilog** - Logging
- **Bootstrap 5** - CSS framework
- **Bootstrap Icons** - Icon library
- **jQuery** - JavaScript library
- **jQuery Validation** - Client-side validation

## References

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Razor Pages](https://docs.microsoft.com/aspnet/core/razor-pages)
- [Bootstrap 5](https://getbootstrap.com)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## License

© 2026 Ecommerce Informatica. All rights reserved.
