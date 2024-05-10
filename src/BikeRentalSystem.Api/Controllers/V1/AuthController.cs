using BikeRentalSystem.Core.Interfaces.Notifications;

namespace BikeRentalSystem.Api.Controllers.V1;

public class AuthController : MainController
{
    private readonly ILogger<AuthController> _logger;
    private readonly INotifier _notifier;

    public AuthController(ILogger<AuthController> logger, INotifier notifier) : base(notifier)
    {
        _logger = logger;
    }
}
