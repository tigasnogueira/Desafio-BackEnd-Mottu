using BikeRentalSystem.Core.Models.Enums;

namespace BikeRentalSystem.Api.Contracts.Request;

public class RentalRequest
{
    public Guid CourierId { get; set; }
    public Guid MotorcycleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public decimal DailyRate { get; set; }
    public RentalPlan Plan { get; set; }
}
