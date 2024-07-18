using BikeRentalSystem.Core.Models.Enums;

namespace BikeRentalSystem.Api.Contracts.Request;

public class RentalRequest
{
    public Guid CourierId { get; init; } = Guid.Empty;
    public Guid MotorcycleId { get; init; } = Guid.Empty;
    public DateOnly StartDate { get; init; } = DateOnly.MinValue;
    public DateOnly EndDate { get; init; } = DateOnly.MinValue;
    public DateOnly ExpectedEndDate { get; init; } = DateOnly.MinValue;
    public decimal DailyRate { get; init; } = 0m;
    public RentalPlan Plan { get; init; } = RentalPlan.SevenDays;
}
