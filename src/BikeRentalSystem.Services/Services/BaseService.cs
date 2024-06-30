using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Services.Services;

public abstract class BaseService
{
    protected readonly INotifier _notifier;

    public BaseService(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected List<string> ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : EntityModel
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
                if (sqlException != null)
                {
                    return $"Database error (Number: {sqlException.Number}, State: {sqlException.State}, Class: {sqlException.Class}): {sqlException.Message}. Please contact support if the issue persists.";
                }
                return $"Database update error: {dbUpdateException.InnerException?.Message ?? dbUpdateException.Message}. Please contact support if the issue persists.";
            default:
                return $"An unexpected error occurred: {exception.Message}. Please try again later or contact support.";
        }
    }
}
