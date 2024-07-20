using BikeRentalSystem.Core.Models.Enums;

namespace BikeRentalSystem.Core.Dtos;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid CourierId { get; set; }
    public Guid MotorcycleId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly ExpectedEndDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public RentalPlan Plan { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedByUser { get; set; }
    public bool IsDeleted { get; set; }

    public CourierDto Courier { get; set; } = new();
    public MotorcycleDto Motorcycle { get; set; } = new();
}
