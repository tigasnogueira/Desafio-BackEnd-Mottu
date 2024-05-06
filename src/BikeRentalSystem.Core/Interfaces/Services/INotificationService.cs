namespace BikeRentalSystem.Core.Interfaces.Services;

public interface INotificationService
{
    Task NotifyAsync(string message);
}
