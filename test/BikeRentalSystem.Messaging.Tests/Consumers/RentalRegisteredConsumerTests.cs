using BikeRentalSystem.Messaging.Consumers;
using BikeRentalSystem.Messaging.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BikeRentalSystem.Messaging.Tests.Consumers;

public class RentalRegisteredConsumerTests
{
    private readonly RentalRegisteredConsumer _consumer;
    private readonly IModel _channel;
    private readonly ILogger<RentalRegisteredConsumer> _logger;

    public RentalRegisteredConsumerTests()
    {
        _channel = Substitute.For<IModel>();
        _logger = Substitute.For<ILogger<RentalRegisteredConsumer>>();
        _consumer = new RentalRegisteredConsumer(_channel, _logger);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenChannelIsNull()
    {
        // Act
        Action act = () => new RentalRegisteredConsumer(null, _logger);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*channel*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act
        Action act = () => new RentalRegisteredConsumer(_channel, null);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
    }

    [Fact]
    public async Task ConsumeAsync_ShouldRegisterConsumerToChannel()
    {
        // Arrange
        EventingBasicConsumer registeredConsumer = null;

        _channel.When(x => x.BasicConsume("rental_queue", false, Arg.Any<IBasicConsumer>()))
                .Do(callInfo =>
                {
                    registeredConsumer = callInfo.Arg<IBasicConsumer>() as EventingBasicConsumer;
                });

        // Act
        await _consumer.ConsumeAsync();

        // Assert
        Assert.NotNull(registeredConsumer);
        _channel.Received(1).BasicConsume("rental_queue", false, Arg.Any<IBasicConsumer>());
    }

    [Fact]
    public async Task EventingBasicConsumer_ShouldProcessMessageSuccessfully()
    {
        // Arrange
        var rentalRegisteredEvent = new RentalRegistered { TotalCost = 100m };
        var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rentalRegisteredEvent));
        var basicDeliverEventArgs = new BasicDeliverEventArgs { Body = new ReadOnlyMemory<byte>(messageBody) };

        EventingBasicConsumer registeredConsumer = null;

        _channel.When(x => x.BasicConsume("rental_queue", false, Arg.Any<IBasicConsumer>()))
                .Do(x => registeredConsumer = x.Arg<IBasicConsumer>() as EventingBasicConsumer);

        // Act
        await _consumer.ConsumeAsync();
        Assert.NotNull(registeredConsumer);

        registeredConsumer.HandleBasicDeliver("consumerTag", 0, false, "exchange", "routingKey", null, basicDeliverEventArgs.Body);

        // Assert
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(v => v.ToString().Contains("Processing RentalRegistered event")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }

    [Fact]
    public async Task EventingBasicConsumer_ShouldLogError_WhenProcessingMessageFails()
    {
        // Arrange
        var invalidMessageBody = Encoding.UTF8.GetBytes("invalid message");
        var basicDeliverEventArgs = new BasicDeliverEventArgs { Body = new ReadOnlyMemory<byte>(invalidMessageBody) };

        EventingBasicConsumer registeredConsumer = null;

        _channel.When(x => x.BasicConsume("rental_queue", false, Arg.Any<IBasicConsumer>()))
                .Do(x => registeredConsumer = x.Arg<IBasicConsumer>() as EventingBasicConsumer);

        // Act
        await _consumer.ConsumeAsync();
        Assert.NotNull(registeredConsumer);

        registeredConsumer.HandleBasicDeliver("consumerTag", 0, false, "exchange", "routingKey", null, basicDeliverEventArgs.Body);

        // Assert
        _logger.Received(1).LogError(Arg.Any<Exception>(), "Error processing message");
    }
}
