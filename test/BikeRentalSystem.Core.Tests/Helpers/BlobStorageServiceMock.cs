using BikeRentalSystem.Core.Interfaces.Services;
using NSubstitute;

namespace BikeRentalSystem.Core.Tests.Helpers;

public static class BlobStorageServiceMock
{
    public static IBlobStorageService Create()
    {
        return Substitute.For<IBlobStorageService>();
    }
}
