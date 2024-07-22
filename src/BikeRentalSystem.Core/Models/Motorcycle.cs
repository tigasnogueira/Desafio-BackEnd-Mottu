namespace BikeRentalSystem.Core.Models;

public class Motorcycle : EntityBase
{
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;

    public virtual MotorcycleNotification? MotorcycleNotification { get; set; }

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
