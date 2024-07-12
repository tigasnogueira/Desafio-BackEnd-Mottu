namespace BikeRentalSystem.Api.Models.Dtos;

public class CourierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; }
    public string CnhType { get; set; }
    public string? CnhImage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
