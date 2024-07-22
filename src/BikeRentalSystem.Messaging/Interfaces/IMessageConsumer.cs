namespace BikeRentalSystem.Messaging.Interfaces;

public interface IMessageConsumer
{
    Task ConsumeAsync();
}
