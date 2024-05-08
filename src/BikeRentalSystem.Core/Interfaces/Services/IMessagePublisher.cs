namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IMessagePublisher
{
    Task PublishAsync(string topic, string message);
}
