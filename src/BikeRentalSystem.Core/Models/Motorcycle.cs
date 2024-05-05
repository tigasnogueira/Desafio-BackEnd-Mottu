namespace BikeRentalSystem.Core.Models;

public class Motorcycle : EntityModel
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public int Mileage { get; set; }
    public int EngineSize { get; set; }
    public string Color { get; set; }
    public string ImageUrl { get; set; }
    public bool IsRented { get; set; }

    public Motorcycle()
    {
        IsRented = false;
    }

    public void Rent()
    {
        IsRented = true;
    }

    public void Return()
    {
        IsRented = false;
    }
}
