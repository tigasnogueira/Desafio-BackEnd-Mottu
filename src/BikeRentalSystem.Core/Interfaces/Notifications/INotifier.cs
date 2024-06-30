using BikeRentalSystem.Core.Notifications;
using FluentValidation.Results;

namespace BikeRentalSystem.Core.Interfaces.Notifications;

public interface INotifier
{
    IReadOnlyList<Notification> GetNotifications();
    bool HasNotification();
    void Handle(Notification notification);
    void Handle(string message);
    void Handle(string message, NotificationType type);
    void Handle(Exception exception);
    void HandleException(Exception exception);
    void NotifyValidationErrors(ValidationResult validationResult);
    void Clean();
}
