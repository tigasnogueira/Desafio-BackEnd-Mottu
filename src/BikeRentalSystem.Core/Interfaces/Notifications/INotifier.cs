using BikeRentalSystem.Core.Notifications;

namespace BikeRentalSystem.Core.Interfaces.Notifications;

public interface INotifier
{
    bool HasNotification();
    List<Notification> GetNotifications();
    void Handle(Notification notification);
    void Handle(string message);
    void Handle(Exception exception);
    void Clean();
}
