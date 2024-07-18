namespace BikeRentalSystem.Api.Contracts.Request;

public class MotorcycleUpdateRequest
{
    public int Year { get; init; } = 0;
    public string Model { get; init; } = string.Empty;
    public string Plate { get; init; } = string.Empty;
}
