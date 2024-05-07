namespace BikeRentalSystem.Api.Dtos;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid CourierId { get; set; }
    public Guid MotorcycleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsFinished { get; set; }
}
