namespace BikeRentalSystem.Core.Models;

public class Motorcycle : EntityBase
{
    public string Identifier { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }

    public Motorcycle()
    {
        IsRented = false;
    }

    public Motorcycle(string identifier, int year, string model, string plate)
    {
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }
}
