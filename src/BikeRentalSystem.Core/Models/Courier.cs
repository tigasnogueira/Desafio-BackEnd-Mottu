namespace BikeRentalSystem.Core.Models;

public class Courier : EntityBase
{
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; }
    public string CnhType { get; set; }
    public string CnhImage { get; set; }

    public Courier()
    {
        IsAvailable = true;
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }

    public Courier(string identifier, string name, string cnpj, DateTime birthDate, string cnhNumber, string cnhType, string cnhImage)
    {
        Identifier = identifier;
        Name = name;
        Cnpj = cnpj;
        BirthDate = birthDate;
        CnhNumber = cnhNumber;
        CnhType = cnhType;
        CnhImage = cnhImage;
    }
}
