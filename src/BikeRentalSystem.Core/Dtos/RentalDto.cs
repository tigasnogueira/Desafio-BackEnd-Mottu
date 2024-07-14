using BikeRentalSystem.Core.Models.Enums;

namespace BikeRentalSystem.Core.Dtos;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid CourierId { get; set; }
    public Guid MotorcycleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public RentalPlan Plan { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public CourierDto Courier { get; set; }
    public MotorcycleDto Motorcycle { get; set; }
}
