using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.DomainTests.Models;

public class MotorcycleTests
{
    [Fact]
    public void Motorcycle_ShouldBeAvailable_WhenCreated()
    {
        var motorcycle = new Motorcycle();
        Assert.False(motorcycle.IsRented);
    }

    [Fact]
    public void Motorcycle_ShouldBeRented_AfterRenting()
    {
        var motorcycle = new Motorcycle();
        motorcycle.Rent();
        Assert.True(motorcycle.IsRented);
    }

    [Fact]
    public void Motorcycle_ShouldBeAvailable_AfterReturning()
    {
        var motorcycle = new Motorcycle();
        motorcycle.Rent();
        motorcycle.Return();
        Assert.False(motorcycle.IsRented);
    }
}
