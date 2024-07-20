using BikeRentalSystem.Messaging.Consumers;
using BikeRentalSystem.Messaging.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BikeRentalSystem.Messaging.Tests.Consumers;

public class CourierRegisteredConsumerTests
{
    private readonly CourierRegisteredConsumer _consumer;
    private readonly IModel _channel;
    private readonly ILogger<CourierRegisteredConsumer> _logger;

    public CourierRegisteredConsumerTests()
    {
        _channel = Substitute.For<IModel>();
        _logger = Substitute.For<ILogger<CourierRegisteredConsumer>>();
        _consumer = new CourierRegisteredConsumer(_channel, _logger);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenChannelIsNull()
    {
        // Act
        Action act = () => new CourierRegisteredConsumer(null, _logger);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*channel*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act
        Action act = () => new CourierRegisteredConsumer(_channel, null);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
    }

    [Fact]
    public async Task ConsumeAsync_ShouldRegisterConsumerToChannel()
    {
        // Arrange
        EventingBasicConsumer capturedConsumer = null;

        _channel.When(x => x.BasicConsume("rental_queue", true, Arg.Any<IBasicConsumer>()))
                .Do(x => capturedConsumer = x.Arg<EventingBasicConsumer>());

        // Act
        await _consumer.ConsumeAsync();

        // Assert
        _channel.Received(1).BasicConsume("rental_queue", true, Arg.Any<IBasicConsumer>());
        capturedConsumer.Should().NotBeNull();
    }

    [Fact]
    public async Task EventingBasicConsumer_ShouldProcessMessageSuccessfully()
    {
        // Arrange
        var courierRegisteredEvent = new CourierRegistered { Name = "Test Courier" };
        var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(courierRegisteredEvent));
        var basicDeliverEventArgs = new BasicDeliverEventArgs
        {
            Body = new ReadOnlyMemory<byte>(messageBody)
        };

        var consumer = new EventingBasicConsumer(_channel);

        _channel.When(x => x.BasicConsume("rental_queue", true, Arg.Any<IBasicConsumer>()))
                .Do(x => consumer = x.Arg<EventingBasicConsumer>());

        // Act
        await _consumer.ConsumeAsync();
        consumer.HandleBasicDeliver("consumerTag", 0, false, "exchange", "routingKey", null, basicDeliverEventArgs.Body);

        // Assert
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains("Processing CourierRegistered event")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }

    [Fact]
    public async Task EventingBasicConsumer_ShouldLogError_WhenProcessingMessageFails()
    {
        // Arrange
        var invalidMessageBody = Encoding.UTF8.GetBytes("invalid message");
        var basicDeliverEventArgs = new BasicDeliverEventArgs
        {
            Body = new ReadOnlyMemory<byte>(invalidMessageBody)
        };

        var consumer = new EventingBasicConsumer(_channel);

        _channel.When(x => x.BasicConsume("rental_queue", true, Arg.Any<IBasicConsumer>()))
                .Do(x => consumer = x.Arg<EventingBasicConsumer>());

        // Act
        await _consumer.ConsumeAsync();
        consumer.HandleBasicDeliver("consumerTag", 0, false, "exchange", "routingKey", null, basicDeliverEventArgs.Body);

        // Assert
        _logger.Received(1).LogError(Arg.Any<Exception>(), "Error processing message");
    }
}
