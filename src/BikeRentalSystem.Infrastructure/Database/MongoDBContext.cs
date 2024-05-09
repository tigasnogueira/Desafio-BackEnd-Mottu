using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Database;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    public IMongoCollection<Motorcycle> Motorcycles => GetCollection<Motorcycle>("Motorcycles");
    public IMongoCollection<Courier> Couriers => GetCollection<Courier>("Couriers");
    public IMongoCollection<Rental> Rentals => GetCollection<Rental>("Rentals");
}
