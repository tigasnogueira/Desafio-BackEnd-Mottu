using BikeRentalSystem.Core.Interfaces.Notifications;

namespace BikeRentalSystem.Core.Notifications;

public class Notifier : INotifier
{
    private List<Notification> _notifications;

    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public bool HasNotification()
    {
        return _notifications.Any();
    }

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void Handle(string message)
    {
        _notifications.Add(new Notification(message));
    }

    public void Handle(Exception exception)
    {
        _notifications.Add(new Notification(exception.Message));
    }

    public void Clean()
    {
        _notifications.Clear();
    }
}
