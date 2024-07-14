namespace BikeRentalSystem.Core.Models;

public class Motorcycle : EntityBase
{
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }

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
