using BikeRentalSystem.Core.Interfaces.Notifications;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

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

        string friendlyMessage = GenerateFriendlyMessage(exception);
        _notifications.Add(new Notification(friendlyMessage, NotificationType.Error));
    }

    private string GenerateFriendlyMessage(Exception exception)
    {
        switch (exception)
        {
            case ArgumentNullException argNullException:
                return $"A required argument was not provided: {argNullException.ParamName}. Please check and try again.";
            case ArgumentException argException:
                return $"There was an issue with the provided argument: {argException.Message}. Please correct it and try again.";
            case InvalidOperationException invalidOpException:
                return $"The operation could not be completed: {invalidOpException.Message}. Please ensure all preconditions are met and try again.";
            case DbUpdateException dbUpdateException:
                var sqlException = dbUpdateException.GetBaseException() as SqlException;
                return sqlException != null ? TranslateSqlException(sqlException) : $"Database update error: {dbUpdateException.InnerException?.Message ?? dbUpdateException.Message}. Please contact support if the issue persists.";
            default:
                return $"An unexpected error occurred: {exception.Message}. Please try again later or contact support.";
        }
    }

    private string TranslateSqlException(SqlException sqlException)
    {
        switch (sqlException.Number)
        {
            case 8152:
                return "The data provided is too long for the database column. Please shorten the input and try again.";
            case 2627:
                return "A unique constraint was violated. This usually means the data you are trying to enter already exists.";
            case 547:
                return "A foreign key constraint was violated. Please ensure all related data exists.";
            case 2601:
                return "A duplicate key error occurred. Please ensure the data is unique and try again.";
            default:
                return $"A database error occurred: {sqlException.Message}. Please contact support if the issue persists.";
        }
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
