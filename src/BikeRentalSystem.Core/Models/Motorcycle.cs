namespace BikeRentalSystem.Core.Models;

public class Motorcycle : EntityBase
{
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;

    public Motorcycle() 
    {
    }

    public Motorcycle(int year, string model, string plate)
    {
        Year = year;
        Model = model;
        Plate = plate;
    }
}
