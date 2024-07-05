namespace BikeRentalSystem.Api.Models.Request;

public class MotorcycleRequest
{
    public string Identifier { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
}
