using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.RentalServices.Services;
using BikeRentalSystem.RentalServices.Tests.Helpers;

namespace BikeRentalSystem.Api.Tests.Helpers;

public static class MotorcycleServiceMock
{
    public static IMotorcycleService Create()
    {
        var unitOfWork = UnitOfWorkMock.Create();
        var messageProducer = MessageProducerMock.Create();
        var notifier = NotifierMock.Create();

        var motorcycleService = new MotorcycleService(unitOfWork, messageProducer, notifier);

        return motorcycleService;
    }
}
