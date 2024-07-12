namespace BikeRentalSystem.Api.Models.Request;

public class CourierUpdateRequest
{
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; } = DateTime.MinValue;
    public string CnhNumber { get; set; }
    public string CnhType { get; set; }
}
