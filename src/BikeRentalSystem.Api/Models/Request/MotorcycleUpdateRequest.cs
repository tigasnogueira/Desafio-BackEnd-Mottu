namespace BikeRentalSystem.Api.Models.Request;

public class MotorcycleUpdateRequest
{
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
}
