namespace BikeRentalSystem.Core.Models;

public class Courier : EntityModel
{
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

    public Courier()
    {
        IsAvailable = true;
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }

    public void SetUnavailable()
    {
        IsAvailable = false;
    }
}
