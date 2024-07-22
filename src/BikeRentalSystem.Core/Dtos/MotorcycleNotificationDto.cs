namespace BikeRentalSystem.Core.Dtos;

public class MotorcycleNotificationDto
{
    public Guid Id { get; set; }
    public Guid MotorcycleId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedByUser { get; set; }
    public bool IsDeleted { get; set; }
}
