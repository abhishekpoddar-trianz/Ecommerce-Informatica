namespace EcommerceInformatica.Application.DTOs;

/// <summary>
/// Data transfer object for Province entity
/// </summary>
public class ProvinceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// DTO for creating a new Province
/// </summary>
public class ProvinceCreateDto
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for updating an existing Province
/// </summary>
public class ProvinceUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
