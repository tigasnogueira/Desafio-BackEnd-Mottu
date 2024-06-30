using BikeRentalSystem.Core.Interfaces.Notifications;
using FluentValidation.Results;

namespace BikeRentalSystem.Core.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications;

    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public IReadOnlyList<Notification> GetNotifications()
    {
        return _notifications.AsReadOnly();
    }

    public bool HasNotification()
    {
        return _notifications.Any();
    }

    public void Handle(Notification notification)
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        _notifications.Add(notification);
    }

    public void Handle(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        _notifications.Add(new Notification(message, NotificationType.Information));
    }

    public void Handle(string message, NotificationType type)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));

        _notifications.Add(new Notification(message, type));
    }

    public void Handle(Exception exception)
    {
        if (exception == null)
            throw new ArgumentNullException(nameof(exception));

        HandleException(exception);
    }

    public void HandleException(Exception exception)
    {
        if (exception == null)
            throw new ArgumentNullException(nameof(exception));

        _notifications.Add(new Notification(exception.Message, NotificationType.Error));
    }

    public void NotifyValidationErrors(ValidationResult validationResult)
    {
        if (validationResult == null)
            throw new ArgumentNullException(nameof(validationResult));

        foreach (var error in validationResult.Errors)
        {
            Handle(error.ErrorMessage, NotificationType.Error);
        }
    }

    public void Clean()
    {
        _notifications.Clear();
    }
}
