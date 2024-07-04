namespace BikeRentalSystem.Core.Models;

public class Rental : EntityBase
{
    public Guid CourierId { get; set; }
    public Guid MotorcycleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public decimal DailyRate { get; set; }

    public virtual Courier Courier { get; set; }
    public virtual Motorcycle Motorcycle { get; set; }

    public Rental()
    {
    }

    public Rental(Guid courierId, Guid motorcycleId, DateTime startDate, DateTime endDate, DateTime expectedEndDate, decimal dailyRate)
    {
        CourierId = courierId;
        MotorcycleId = motorcycleId;
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
        DailyRate = dailyRate;
    }
}
