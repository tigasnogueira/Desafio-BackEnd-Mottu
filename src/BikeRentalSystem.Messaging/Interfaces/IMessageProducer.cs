namespace BikeRentalSystem.Messaging.Interfaces;

public interface IMessageProducer
{
    void Publish<T>(T message, string exchange, string routingKey) where T : class;
}
