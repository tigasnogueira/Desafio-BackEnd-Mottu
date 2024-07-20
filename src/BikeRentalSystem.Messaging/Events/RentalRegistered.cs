using BikeRentalSystem.Core.Models.Enums;

namespace BikeRentalSystem.Messaging.Events;

public class RentalRegistered
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CourierId { get; set; }
    public Guid MotorcycleId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly ExpectedEndDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public RentalPlan Plan { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedByUser { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedByUser { get; set; }
    public bool IsDeleted { get; set; } = false;
}
