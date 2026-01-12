namespace EcommerceInformatica.Application.DTOs;

/// <summary>
/// Data transfer object for InvoiceDetail entity
/// </summary>
public class InvoiceDetailDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string? InvoiceNumber { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// DTO for creating a new InvoiceDetail
/// </summary>
public class InvoiceDetailCreateDto
{
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for updating an existing InvoiceDetail
/// </summary>
public class InvoiceDetailUpdateDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public bool IsActive { get; set; }
}
