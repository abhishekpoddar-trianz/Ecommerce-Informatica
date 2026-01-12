namespace EcommerceInformatica.Application.DTOs;

/// <summary>
/// Data transfer object for Invoice entity
/// </summary>
public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public string? PersonName { get; set; }
    public int PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
    public List<InvoiceDetailDto>? InvoiceDetails { get; set; }
}

/// <summary>
/// DTO for creating a new Invoice
/// </summary>
public class InvoiceCreateDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; } = true;
    public List<InvoiceDetailCreateDto>? InvoiceDetails { get; set; }
}

/// <summary>
/// DTO for updating an existing Invoice
/// </summary>
public class InvoiceUpdateDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; }
}
