using BikeRentalSystem.Core.Interfaces.Notifications;
using NSubstitute;

namespace BikeRentalSystem.Core.Tests.Helpers;

public static class NotifierMock
{
    public static INotifier Create()
    {
        return Substitute.For<INotifier>();
    }
}
