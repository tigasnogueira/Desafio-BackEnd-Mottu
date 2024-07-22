namespace BikeRentalSystem.Core.Models;

public class MotorcycleNotification : EntityBase
{
    public Guid MotorcycleId { get; set; }
    public string Message { get; set; } = string.Empty;

    public virtual Motorcycle Motorcycle { get; set; } = null!;
}
