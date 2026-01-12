namespace EcommerceInformatica.Domain.Entities;

/// <summary>
/// Represents a city
/// </summary>
public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProvinceId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Province? Province { get; set; }
    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
