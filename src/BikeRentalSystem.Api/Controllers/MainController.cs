using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BikeRentalSystem.Api.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    private readonly INotifier _notifier;

    protected MainController(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected ActionResult CustomResponse(object result = null, int? statusCode = null)
    {
        if (ValidOperation())
        {
            if (statusCode.HasValue)
            {
                return StatusCode(statusCode.Value, new { success = true, data = result });
            }

            return result == null ? NoContent() : Ok(new { success = true, data = result });
        }

        return HandleErrorResponse();
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
        {
            NotifyErrorInvalidModel(modelState);
        }

        return CustomResponse();
    }

    protected bool ValidOperation() => !_notifier.HasNotification();

    protected void NotifyErrorInvalidModel(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors)
        {
            var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
            NotifyError(errorMsg);
        }
    }

    protected void NotifyError(string message) => _notifier.Handle(new Notification(message));

    protected ActionResult HandleErrorResponse()
    {
        var errors = _notifier.GetNotifications().Select(n => n.Message);

        if (IsNotFoundError(errors))
        {
            return NotFound(new { success = false, errors });
        }

        if (IsBadRequestError(errors))
        {
            return BadRequest(new { success = false, errors });
        }

        // Fallback para outros tipos de erro
        return StatusCode(500, new { success = false, errors, message = "An unexpected error occurred." });
    }

    protected static bool IsNotFoundError(IEnumerable<string> errors) =>
        errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase));

    protected static bool IsBadRequestError(IEnumerable<string> errors) =>
        errors.Any(e => e.Contains("invalid", StringComparison.OrdinalIgnoreCase) || e.Contains("cannot", StringComparison.OrdinalIgnoreCase));
}
