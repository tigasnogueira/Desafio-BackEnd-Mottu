using BikeRentalSystem.Messaging.Interfaces;
using NSubstitute;

namespace BikeRentalSystem.RentalServices.Tests.Helpers;

public static class MessageProducerMock
{
    public static IMessageProducer Create()
    {
        return Substitute.For<IMessageProducer>();
    }
}
