# PowerShell Script to Generate CRUD Pages for Ecommerce Informatica
# Usage: .\Generate-CRUDPages.ps1

$entities = @(
    @{ Name = "Category"; Plural = "Categories"; Icon = "grid"; Fields = @("Name", "Description") },
    @{ Name = "Province"; Plural = "Provinces"; Icon = "geo-alt"; Fields = @("Name") },
    @{ Name = "PaymentMethod"; Plural = "PaymentMethods"; Icon = "credit-card"; Fields = @("Name", "Description") },
    @{ Name = "Supplier"; Plural = "Suppliers"; Icon = "building"; Fields = @("Name", "ContactPerson", "Email", "Phone", "Address") },
    @{ Name = "City"; Plural = "Cities"; Icon = "geo"; Fields = @("Name"); Relations = @("Province") },
    @{ Name = "Person"; Plural = "Persons"; Icon = "people"; Fields = @("FirstName", "LastName", "Email", "Phone", "Address", "Username"); Relations = @("City") },
    @{ Name = "Invoice"; Plural = "Invoices"; Icon = "receipt"; Fields = @("InvoiceNumber", "InvoiceDate", "TotalAmount"); Relations = @("Person", "PaymentMethod") },
    @{ Name = "InvoiceDetail"; Plural = "InvoiceDetails"; Icon = "list-check"; Fields = @("Quantity", "UnitPrice", "Subtotal"); Relations = @("Invoice", "Product") }
)

function New-IndexPage {
    param (
        [string]$EntityName,
        [string]$PluralName,
        [string]$Icon
    )

    $content = @"
@page
@model EcommerceInformatica.Web.Pages.$EntityName.IndexModel
@{
    ViewData["Title"] = "$PluralName";
}

<div class="container-fluid mt-4">
    <div class="row mb-3">
        <div class="col-md-6">
            <h2><i class="bi bi-$Icon"></i> $PluralName</h2>
        </div>
        <div class="col-md-6 text-end">
            <a asp-page="Create" class="btn btn-primary">
                <i class="bi bi-plus-circle"></i> Create New $EntityName
            </a>
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0"><i class="bi bi-search"></i> Search $PluralName</h5>
        </div>
        <div class="card-body">
            <form method="get">
                <div class="row">
                    <div class="col-md-10">
                        <input type="text" name="searchString" value="@Model.SearchString"
                               class="form-control" placeholder="Search..." />
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
            @if (Model.$PluralName.Any())
            {
                <div class="table-responsive">
                    <table class="table table-striped table-hover">
                        <thead class="table-primary">
                            <tr>
                                <th>Name</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var item in Model.$PluralName)
                            {
                                <tr>
                                    <td>@item.Name</td>
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
                    <i class="bi bi-info-circle"></i> No $PluralName.ToLower() found.
                </div>
            }
        </div>
    </div>
</div>
"@

    return $content
}

# Generate pages for each entity
foreach ($entity in $entities) {
    $entityName = $entity.Name
    $pluralName = $entity.Plural
    $icon = $entity.Icon

    # Create directory
    $dir = "Pages\$entityName"
    if (-not (Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
    }

    # Generate Index.cshtml
    $indexContent = New-IndexPage -EntityName $entityName -PluralName $pluralName -Icon $icon
    $indexContent | Out-File -FilePath "$dir\Index.cshtml" -Encoding UTF8

    Write-Host "Generated $dir\Index.cshtml" -ForegroundColor Green
}

Write-Host "`nCRUD page generation complete!" -ForegroundColor Cyan
Write-Host "Note: This script generates only Index pages. Use templates in CRUD_GENERATION_GUIDE.md for other pages." -ForegroundColor Yellow
