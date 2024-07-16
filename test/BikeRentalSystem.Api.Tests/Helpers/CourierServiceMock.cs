using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.RentalServices.Services;
using BikeRentalSystem.RentalServices.Tests.Helpers;

namespace BikeRentalSystem.Api.Tests.Helpers;

public static class CourierServiceMock
{
    public static ICourierService Create()
    {
        var unitOfWork = UnitOfWorkMock.Create();
        var messageProducer = MessageProducerMock.Create();
        var notifier = NotifierMock.Create();

        var courierService = new CourierService(unitOfWork, messageProducer, notifier);

        return courierService;
    }
}
