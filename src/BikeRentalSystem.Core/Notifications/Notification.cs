namespace BikeRentalSystem.Core.Notifications;

public class Notification
{
    public Notification(string message, NotificationType type)
    {
        Message = message;
        Type = type;
    }

    public string Message { get; }
    public NotificationType Type { get; set; }
}

public enum NotificationType
{
    Information,
    Error
}
