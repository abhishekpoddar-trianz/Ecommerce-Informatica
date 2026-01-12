namespace EcommerceInformatica.Application.DTOs;

/// <summary>
/// Data transfer object for Product entity
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string ArticleId { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public int BrandId { get; set; }
    public string? BrandName { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// DTO for creating a new Product
/// </summary>
public class ProductCreateDto
{
    public string ArticleId { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for updating an existing Product
/// </summary>
public class ProductUpdateDto
{
    public int Id { get; set; }
    public string ArticleId { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; }
}
