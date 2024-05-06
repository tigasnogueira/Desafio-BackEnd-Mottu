namespace BikeRentalSystem.Core.Models;

public class Rental : EntityModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
    public bool IsFinished { get; set; }
    public Guid MotorcycleId { get; set; }
    public Motorcycle Motorcycle { get; set; }
    public Guid CourierId { get; set; }
    public Courier Courier { get; set; }

    public Rental()
    {
        IsPaid = false;
        IsFinished = false;
    }

    public void Pay()
    {
        IsPaid = true;
    }

    public void Finish()
    {
        IsFinished = true;
    }

    public void SetMotorcycle(Guid motorcycleId)
    {
        MotorcycleId = motorcycleId;
    }

    public void SetCourier(Guid courierId)
    {
        CourierId = courierId;
    }

    public void SetDates(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public void SetPrice(decimal price)
    {
        Price = price;
    }
}
