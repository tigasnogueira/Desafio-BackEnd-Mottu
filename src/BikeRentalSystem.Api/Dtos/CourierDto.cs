namespace BikeRentalSystem.Api.Dtos;

public class CourierDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CNPJ { get; set; }
    public DateTime BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; }
    public string DriverLicenseType { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
}
