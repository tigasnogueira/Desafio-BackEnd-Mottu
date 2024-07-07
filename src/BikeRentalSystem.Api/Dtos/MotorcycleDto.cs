namespace BikeRentalSystem.Api.Dtos;

public class MotorcycleDto
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public int Mileage { get; set; }
    public int EngineSize { get; set; }
    public string Color { get; set; }
    public string ImageUrl { get; set; }
    public bool IsRented { get; set; }
}
