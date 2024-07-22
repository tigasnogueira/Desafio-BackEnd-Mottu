using BikeRentalSystem.Messaging.Interfaces;
using BikeRentalSystem.Messaging.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BikeRentalSystem.Messaging.Tests.Services;

public class RabbitMQConsumerHostedServiceTests
{
    private readonly IEnumerable<IMessageConsumer> _consumers;
    private readonly ILogger<RabbitMQHostedServiceConsumer> _logger;
    private readonly RabbitMQHostedServiceConsumer _service;

    public RabbitMQConsumerHostedServiceTests()
    {
        _consumers = new List<IMessageConsumer>
        {
            Substitute.For<IMessageConsumer>(),
            Substitute.For<IMessageConsumer>()
        };

        _logger = Substitute.For<ILogger<RabbitMQHostedServiceConsumer>>();
        _service = new RabbitMQHostedServiceConsumer(_consumers, _logger);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConsumersIsNull()
    {
        // Act
        Action act = () => new RabbitMQHostedServiceConsumer(null, _logger);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*consumers*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act
        Action act = () => new RabbitMQHostedServiceConsumer(_consumers, null);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldInvokeConsumeAsyncOnAllConsumers()
    {
        // Arrange
        var stoppingToken = new CancellationToken();
        foreach (var consumer in _consumers)
        {
            consumer.ConsumeAsync().Returns(Task.CompletedTask);
        }

        // Act
        await _service.StartAsync(stoppingToken);

        // Assert
        foreach (var consumer in _consumers)
        {
            await consumer.Received(1).ConsumeAsync();
        }
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleExceptionsFromConsumers()
    {
        // Arrange
        var stoppingToken = new CancellationToken();
        var failingConsumer = Substitute.For<IMessageConsumer>();
        failingConsumer.ConsumeAsync().Returns(Task.FromException(new Exception("Consume failed")));

        var serviceWithFailingConsumer = new RabbitMQHostedServiceConsumer(new List<IMessageConsumer> { failingConsumer }, _logger);

        // Act
        Func<Task> act = async () => await serviceWithFailingConsumer.StartAsync(stoppingToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();

        // Verify logging
        _logger.Received(1).LogError(Arg.Any<Exception>(), "Error occurred while consuming messages.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStopGracefully_WhenCancellationIsRequested()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        foreach (var consumer in _consumers)
        {
            consumer.ConsumeAsync().Returns(Task.CompletedTask);
        }

        // Act
        await _service.StartAsync(cts.Token);

        // Assert
        foreach (var consumer in _consumers)
        {
            await consumer.Received(1).ConsumeAsync();
        }
    }
}
