using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Messaging.Consumers;
using BikeRentalSystem.Messaging.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq.Expressions;
using System.Text;

namespace BikeRentalSystem.Messaging.Tests.Consumers;

public class MotorcycleRegisteredConsumerTests
{
    private readonly MotorcycleRegisteredConsumer _consumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IModel _channel;
    private readonly ILogger<MotorcycleRegisteredConsumer> _logger;

    public MotorcycleRegisteredConsumerTests()
    {
        _channel = Substitute.For<IModel>();
        _logger = Substitute.For<ILogger<MotorcycleRegisteredConsumer>>();
        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        _scope = Substitute.For<IServiceScope>();
        _serviceProvider = Substitute.For<IServiceProvider>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _serviceScopeFactory.CreateScope().Returns(_scope);
        _scope.ServiceProvider.Returns(_serviceProvider);
        _serviceProvider.GetService(typeof(IUnitOfWork)).Returns(_unitOfWork);

        _consumer = new MotorcycleRegisteredConsumer(_channel, _logger, _serviceScopeFactory);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenChannelIsNull()
    {
        // Act
        Action act = () => new MotorcycleRegisteredConsumer(null, _logger, _serviceScopeFactory);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*channel*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act
        Action act = () => new MotorcycleRegisteredConsumer(_channel, null, _serviceScopeFactory);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
    }

    [Fact]
    public async Task ConsumeAsync_ShouldRegisterConsumerToChannel()
    {
        // Arrange
        EventingBasicConsumer registeredConsumer = null;

        _channel.When(x => x.BasicConsume("motorcycle_queue", false, Arg.Any<IBasicConsumer>()))
                .Do(callInfo =>
                {
                    registeredConsumer = callInfo.Arg<IBasicConsumer>() as EventingBasicConsumer;
                });

        // Act
        await _consumer.ConsumeAsync();

        // Assert
        Assert.NotNull(registeredConsumer);
        _channel.Received(1).BasicConsume("motorcycle_queue", false, Arg.Any<IBasicConsumer>());
    }

    [Fact]
    public async Task EventingBasicConsumer_ShouldProcessMessageSuccessfully()
    {
        // Arrange
        var motorcycleRegisteredEvent = new MotorcycleRegistered { Id = Guid.NewGuid(), Year = 2024, Model = "Test Model" };
        var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(motorcycleRegisteredEvent));
        var basicDeliverEventArgs = new BasicDeliverEventArgs { Body = new ReadOnlyMemory<byte>(messageBody) };

        EventingBasicConsumer registeredConsumer = null;

        _channel.When(x => x.BasicConsume("motorcycle_queue", false, Arg.Any<IBasicConsumer>()))
                .Do(x => registeredConsumer = x.Arg<IBasicConsumer>() as EventingBasicConsumer);

        // Configurar o mock da unidade de trabalho
        var mockNotificationRepo = Substitute.For<IMotorcycleNotificationRepository>();
        mockNotificationRepo.Find(Arg.Any<Expression<Func<MotorcycleNotification, bool>>>()).Returns(new List<MotorcycleNotification>());

        _unitOfWork.MotorcycleNotifications.Returns(mockNotificationRepo);

        // Act
        await _consumer.ConsumeAsync();
        Assert.NotNull(registeredConsumer);

        registeredConsumer.HandleBasicDeliver("consumerTag", 0, false, "motorcycle_exchange", "motorcycle_routingKey", null, basicDeliverEventArgs.Body);

        // Assert
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains("Processing MotorcycleRegistered event")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );

        await _unitOfWork.MotorcycleNotifications.Received(1).Add(Arg.Is<MotorcycleNotification>(n => n.MotorcycleId == motorcycleRegisteredEvent.Id));
        await _unitOfWork.Received(1).SaveAsync();
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

        EventingBasicConsumer registeredConsumer = null;

        _channel.When(x => x.BasicConsume("motorcycle_queue", false, Arg.Any<IBasicConsumer>()))
                .Do(x => registeredConsumer = x.Arg<IBasicConsumer>() as EventingBasicConsumer);

        // Act
        await _consumer.ConsumeAsync();
        Assert.NotNull(registeredConsumer);

        registeredConsumer.HandleBasicDeliver("consumerTag", 0, false, "motorcycle_exchange", "motorcycle_routingKey", null, basicDeliverEventArgs.Body);

        // Assert
        _logger.Received(1).LogError(Arg.Any<Exception>(), "Error processing message");
    }
}
