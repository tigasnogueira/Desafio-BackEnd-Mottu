namespace BikeRentalSystem.Core.Dtos;

public class MotorcycleDto
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedByUser { get; set; }
    public bool IsDeleted { get; set; }

    public MotorcycleNotificationDto MotorcycleNotification { get; set; } = new();
}
