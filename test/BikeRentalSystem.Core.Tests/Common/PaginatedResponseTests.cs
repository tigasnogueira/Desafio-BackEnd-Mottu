using BikeRentalSystem.Core.Common;
using FluentAssertions;

namespace BikeRentalSystem.Core.Tests.Common;

public class PaginatedResponseTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializePropertiesCorrectly()
    {
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var count = 3;
        var pageNumber = 1;
        var pageSize = 2;

        var paginatedResponse = new PaginatedResponse<string>(items, count, pageNumber, pageSize);

        paginatedResponse.Items.Should().BeEquivalentTo(items);
        paginatedResponse.TotalItems.Should().Be(count);
        paginatedResponse.PageNumber.Should().Be(pageNumber);
        paginatedResponse.PageSize.Should().Be(pageSize);
        paginatedResponse.TotalPages.Should().Be(2);
    }

    [Fact]
    public void Constructor_WithEmptyItems_ShouldInitializePropertiesCorrectly()
    {
        var items = new List<string>();
        var count = 0;
        var pageNumber = 1;
        var pageSize = 2;

        var paginatedResponse = new PaginatedResponse<string>(items, count, pageNumber, pageSize);

        paginatedResponse.Items.Should().BeEquivalentTo(items);
        paginatedResponse.TotalItems.Should().Be(count);
        paginatedResponse.PageNumber.Should().Be(pageNumber);
        paginatedResponse.PageSize.Should().Be(pageSize);
        paginatedResponse.TotalPages.Should().Be(0);
    }

    [Fact]
    public void Constructor_WithNullPageNumberAndPageSize_ShouldInitializePropertiesCorrectly()
    {
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var count = 3;

        var paginatedResponse = new PaginatedResponse<string>(items, count, null, null);

        paginatedResponse.Items.Should().BeEquivalentTo(items);
        paginatedResponse.TotalItems.Should().Be(count);
        paginatedResponse.PageNumber.Should().BeNull();
        paginatedResponse.PageSize.Should().BeNull();
        paginatedResponse.TotalPages.Should().Be(0);
    }

    [Fact]
    public void TotalPages_ShouldCalculateCorrectly_WhenCountIsNotDivisibleByPageSize()
    {
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var count = 5;
        var pageNumber = 1;
        var pageSize = 2;

        var paginatedResponse = new PaginatedResponse<string>(items, count, pageNumber, pageSize);

        paginatedResponse.TotalPages.Should().Be(3);
    }

    [Fact]
    public void Items_ShouldBeInitialized_WhenDefaultConstructorIsUsed()
    {
        var paginatedResponse = new PaginatedResponse<string>();

        paginatedResponse.Items.Should().NotBeNull();
        paginatedResponse.Items.Should().BeEmpty();
    }
}
