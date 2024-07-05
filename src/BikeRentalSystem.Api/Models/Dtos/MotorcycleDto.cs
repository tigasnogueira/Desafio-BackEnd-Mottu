namespace BikeRentalSystem.Api.Models.Dtos;

public class MotorcycleDto
{
    public Guid Id { get; set; }
    public string Identifier { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
