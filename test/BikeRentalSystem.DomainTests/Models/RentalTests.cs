using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.DomainTests.Models;

public class RentalTests
{
    [Fact]
    public void Rental_ShouldBeUnpaidAndUnfinished_WhenCreated()
    {
        var rental = new Rental();
        Assert.False(rental.IsPaid);
        Assert.False(rental.IsFinished);
    }

    [Fact]
    public void Rental_ShouldBePaid_AfterPaying()
    {
        var rental = new Rental();
        rental.Pay();
        Assert.True(rental.IsPaid);
    }

    [Fact]
    public void Rental_ShouldBeFinished_AfterFinishing()
    {
        var rental = new Rental();
        rental.Finish();
        Assert.True(rental.IsFinished);
    }
}
