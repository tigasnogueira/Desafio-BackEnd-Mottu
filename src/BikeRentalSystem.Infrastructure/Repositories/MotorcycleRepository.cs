using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    private readonly IMongoCollection<Motorcycle> _collection;

    public MotorcycleRepository(IMongoDatabase database)
        : base(database, "motorcycles")
    {
        _collection = database.GetCollection<Motorcycle>("motorcycles");
    }

    public IEnumerable<Motorcycle> GetAvailableMotorcycles()
    {
        return _collection.Find(e => !e.IsRented).ToList();
    }

    public IEnumerable<Motorcycle> GetRentedMotorcycles()
    {
        return _collection.Find(e => e.IsRented).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByBrand(string brand)
    {
        return _collection.Find(e => e.Brand == brand).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByModel(string model)
    {
        return _collection.Find(e => e.Model == model).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByYear(int year)
    {
        return _collection.Find(e => e.Year == year).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByColor(string color)
    {
        return _collection.Find(e => e.Color == color).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByEngineSize(int engineSize)
    {
        return _collection.Find(e => e.EngineSize == engineSize).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByMileage(int mileage)
    {
        return _collection.Find(e => e.Mileage == mileage).ToList();
    }

    public IEnumerable<Motorcycle> GetMotorcyclesByLicensePlate(string licensePlate)
    {
        return _collection.Find(e => e.LicensePlate == licensePlate).ToList();
    }
}
