namespace BikeRentalSystem.Api.Models.Request;

public class CourierRequest
{
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; }
    public string CnhType { get; set; }
    public string CnhImage { get; set; }
}
