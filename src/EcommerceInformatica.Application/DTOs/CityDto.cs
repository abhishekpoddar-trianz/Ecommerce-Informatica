namespace EcommerceInformatica.Application.DTOs;

/// <summary>
/// Data transfer object for City entity
/// </summary>
public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProvinceId { get; set; }
    public string? ProvinceName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// DTO for creating a new City
/// </summary>
public class CityCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int ProvinceId { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for updating an existing City
/// </summary>
public class CityUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProvinceId { get; set; }
    public bool IsActive { get; set; }
}
