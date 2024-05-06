using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Notifications;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class NotificationService : INotificationService
{
    private readonly INotifier _notifier;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotifier notifier, ILogger<NotificationService> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task NotifyAsync(string message)
    {
        try
        {
            var notification = new Notification(message);
            _notifier.Handle(notification);
            await Task.CompletedTask;
            _logger.LogInformation("Notification sent: " + message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send notification: " + ex.Message);
            throw;
        }
    }
}
