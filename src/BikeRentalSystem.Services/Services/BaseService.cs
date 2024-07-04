using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace BikeRentalSystem.RentalServices.Services;

public class BaseService
{
    protected readonly INotifier _notifier;

    public BaseService(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected List<string> ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : EntityBase
    {
        var validator = validation.ValidateAsync(entity);
        var errors = new List<string>();

        if (validator.Result.IsValid) return errors;

        foreach (var error in validator.Result.Errors)
        {
            var errorMessage = $"Property: {error.PropertyName} - Error: {error.ErrorMessage}";
            _notifier.Handle(errorMessage);
            errors.Add(errorMessage);
        }

        return errors;
    }

    protected void HandleException(Exception exception)
    {
        if (exception == null)
            throw new ArgumentNullException(nameof(exception));

        string friendlyMessage = GenerateFriendlyMessage(exception);
        _notifier.Handle(friendlyMessage, NotificationType.Error);
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
}
