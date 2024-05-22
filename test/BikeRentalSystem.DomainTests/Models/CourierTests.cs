using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.DomainTests.Models;

public class CourierTests
{
    [Fact]
    public void Courier_ShouldBeAvailable_WhenCreated()
    {
        var courier = new Courier();
        Assert.True(courier.IsAvailable);
    }

    [Fact]
    public void Courier_ShouldBeUnavailable_AfterSettingUnavailable()
    {
        var courier = new Courier();
        courier.SetUnavailable();
        Assert.False(courier.IsAvailable);
    }

    [Fact]
    public void Courier_ShouldBeAvailable_AfterSettingAvailable()
    {
        var courier = new Courier();
        courier.SetUnavailable();
        courier.SetAvailable();
        Assert.True(courier.IsAvailable);
    }
}
