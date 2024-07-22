using BikeRentalSystem.Messaging.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RabbitMQ.Client;

namespace BikeRentalSystem.Messaging.Tests.Services;

public class RabbitMQProducerTests
{
    private readonly RabbitMQProducer _rabbitMQProducer;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQProducer> _logger;

    public RabbitMQProducerTests()
    {
        _channel = Substitute.For<IModel>();
        _logger = Substitute.For<ILogger<RabbitMQProducer>>();
        _rabbitMQProducer = new RabbitMQProducer(_channel, _logger);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenChannelIsNull()
    {
        Action act = () => new RabbitMQProducer(null, _logger);
        act.Should().Throw<ArgumentNullException>().WithMessage("*channel*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        Action act = () => new RabbitMQProducer(_channel, null);
        act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
    }

    [Fact]
    public async Task PublishAsync_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        string exchange = "testExchange";
        string routingKey = "testRoutingKey";

        Func<Task> act = async () => await _rabbitMQProducer.PublishAsync<object>(null, exchange, routingKey);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("*message*");
    }

    [Fact]
    public async Task PublishAsync_Should_LogInformation_When_MessagePublishedSuccessfully()
    {
        // Arrange
        var channel = Substitute.For<IModel>();
        var logger = Substitute.For<ILogger<RabbitMQProducer>>();
        var producer = new RabbitMQProducer(channel, logger);
        var message = new { Name = "Test" };
        var exchange = "test_exchange";
        var routingKey = "test_routing_key";

        // Act
        await producer.PublishAsync(message, exchange, routingKey);

        // Assert
        logger.Received(1).Log(
            LogLevel.Information,
            0,
            Arg.Is<object>(v => v.ToString() == $"Message published to exchange {exchange} with routing key {routingKey}"),
            null,
            Arg.Any<Func<object, Exception, string>>());
    }

    [Fact]
    public async void PublishAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange
        var channel = Substitute.For<IModel>();
        var logger = Substitute.For<ILogger<RabbitMQProducer>>();
        var producer = new RabbitMQProducer(channel, logger);

        // Act
        Func<Task> act = () => producer.PublishAsync<object>(null, "exchange", "routingKey");

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task PublishAsync_Should_LogErrorAndRethrow_When_ExceptionIsThrown()
    {
        // Arrange
        var channel = Substitute.For<IModel>();
        var logger = Substitute.For<ILogger<RabbitMQProducer>>();
        var producer = new RabbitMQProducer(channel, logger);
        var message = new { Name = "Test" };
        var exchange = "test_exchange";
        var routingKey = "test_routing_key";
        var exception = new Exception("Test exception");

        channel
            .When(x => x.BasicPublish(exchange, routingKey, null, Arg.Any<ReadOnlyMemory<byte>>()))
            .Do(x => throw exception);

        // Act
        Func<Task> act = () => producer.PublishAsync(message, exchange, routingKey);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        logger.Received(1).Log(
            LogLevel.Error,
            0,
            Arg.Is<object>(v => v.ToString() == $"Error publishing message to exchange {exchange} with routing key {routingKey}"),
            exception,
            Arg.Any<Func<object, Exception, string>>());
    }
}
