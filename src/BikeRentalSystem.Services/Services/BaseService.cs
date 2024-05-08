using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalSystem.Services.Services;

public abstract class BaseService
{
    private readonly INotifier _notifier;

    protected BaseService(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.ErrorMessage)
        {
            Notify(error.ToString());
        }
    }

    protected void Notify(string message)
    {
        _notifier.Handle(new Notification(message));
    }

    protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : EntityModel
    {
        var validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator.ToString());

        return false;
    }
}
