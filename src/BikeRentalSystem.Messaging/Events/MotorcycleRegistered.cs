namespace BikeRentalSystem.Messaging.Events;

public class MotorcycleRegistered
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
