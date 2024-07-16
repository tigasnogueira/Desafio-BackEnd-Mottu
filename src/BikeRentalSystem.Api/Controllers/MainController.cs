using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BikeRentalSystem.Api.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    protected readonly INotifier _notifier;
    protected readonly IUser _user;

    protected MainController(INotifier notifier, IUser user)
    {
        _notifier = notifier;
        _user = user;
    }

    protected ActionResult CustomResponse(object? result = null, int? statusCode = null)
    {
        if (!ValidOperation())
            return HandleErrorResponse();

        if (statusCode.HasValue)
        {
            if (statusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(new { success = false, errors = result ?? "Resource not found" });
            }
            if (statusCode == StatusCodes.Status201Created)
            {
                return StatusCode(StatusCodes.Status201Created, new { success = true, data = result });
            }
            if (statusCode == StatusCodes.Status204NoContent)
            {
                return NoContent();
            }
            if (statusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(new { success = false, errors = result ?? "Bad request" });
            }
            return StatusCode(statusCode.Value, new { success = true, data = result });
        }

        if (result is string errorMessage && HttpContext.Request.Method != "GET")
        {
            return BadRequest(new { success = false, errors = errorMessage });
        }

        switch (HttpContext.Request.Method)
        {
            case "GET":
                return result == null ? NotFound(new { success = false, errors = "Resource not found" }) : Ok(new { success = true, data = result });
            case "POST":
                return result == null ? Conflict(new { success = false, errors = "Resource conflict" }) : StatusCode(201, new { success = true, data = result });
            case "PUT":
            case "PATCH":
                return result == null ? NotFound(new { success = false, errors = "Resource not found" }) : Ok(new { success = true, data = result });
            case "DELETE":
                return NoContent();
            default:
                return Ok(new { success = true, data = result });
        }
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (modelState != null && !modelState.IsValid)
        {
            NotifyErrorInvalidModel(modelState);
            return BadRequest(new
            {
                success = false,
                errors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        return CustomResponse();
    }

    protected bool ValidOperation() => !_notifier.GetNotifications().Any(n => n.Type == NotificationType.Error);

    private void NotifyErrorInvalidModel(ModelStateDictionary modelState)
    {
        if (modelState == null) return;

        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors)
        {
            var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
            var errorType = NotificationType.Error;
            NotifyError(errorMsg, errorType);
        }
    }

    protected void NotifyError(string message, NotificationType type = NotificationType.Error) => _notifier.Handle(new Notification(message, type));

    protected void HandleException(Exception exception) => _notifier.HandleException(exception);

    private ActionResult HandleErrorResponse()
    {
        var errors = _notifier.GetNotifications()
            .Where(n => n.Type == NotificationType.Error)
            .Select(n => n.Message)
            .ToList();

        if (IsNotFoundError(errors))
        {
            return NotFound(new { success = false, errors });
        }

        if (IsBadRequestError(errors))
        {
            return BadRequest(new { success = false, errors });
        }

        return StatusCode(500, new { success = false, errors, message = "An unexpected error occurred." });
    }

    private static bool IsNotFoundError(IEnumerable<string> errors) =>
        errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase));

    private static bool IsBadRequestError(IEnumerable<string> errors) =>
        errors.Any(e =>
            e.Contains("invalid", StringComparison.OrdinalIgnoreCase) ||
            e.Contains("cannot", StringComparison.OrdinalIgnoreCase) ||
            e.Contains("Error", StringComparison.OrdinalIgnoreCase));
}
