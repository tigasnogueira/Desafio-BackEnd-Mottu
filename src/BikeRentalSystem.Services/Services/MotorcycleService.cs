using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Messaging.Events;
using BikeRentalSystem.Messaging.Interfaces;

namespace BikeRentalSystem.RentalServices.Services;

public class MotorcycleService : BaseService, IMotorcycleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageProducer _messageProducer;

    public MotorcycleService(IUnitOfWork unitOfWork, IMessageProducer messageProducer, INotifier notifier) : base(notifier)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
    }

    public async Task<Motorcycle> GetById(Guid id)
    {
        try
        {
            _notifier.Handle("Getting motorcycle by ID");
            return await _unitOfWork.Motorcycles.GetById(id);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAll()
    {
        try
        {
            _notifier.Handle("Getting all motorcycles");
            return await _unitOfWork.Motorcycles.GetAll();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<PaginatedResponse<Motorcycle>> GetAllPaged(int page, int pageSize)
    {
        try
        {
            _notifier.Handle("Getting paged motorcycles");
            return await _unitOfWork.Motorcycles.GetAllPaged(page, pageSize);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<Motorcycle> GetByPlate(string plate)
    {
        try
        {
            _notifier.Handle("Getting motorcycle by plate");
            return await _unitOfWork.Motorcycles.GetByPlate(plate);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAllByYear(int year)
    {
        try
        {
            _notifier.Handle("Getting all motorcycles by year");
            return await _unitOfWork.Motorcycles.GetAllByYear(year);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<MotorcycleNotification> GetMotorcycleNotification(Guid id)
    {
        try
        {
            _notifier.Handle("Getting motorcycle notification by ID");
            return await _unitOfWork.MotorcycleNotifications.GetByMotorcycleId(id);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<bool> Add(Motorcycle motorcycle, string userEmail)
    {
        if (motorcycle == null)
        {
            _notifier.Handle("Motorcycle details cannot be null", NotificationType.Error);
            return false;
        }

        var validator = new MotorcycleValidation(_unitOfWork);
        validator.ConfigureRulesForCreate();

        var validationResult = await validator.ValidateAsync(motorcycle);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                motorcycle.CreatedByUser = userEmail;

                await _unitOfWork.Motorcycles.Add(motorcycle);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Motorcycle added successfully");

                    PublishMotorcycleRegisteredEvent(motorcycle);
                    return true;
                }

                await transaction.RollbackAsync();
                _notifier.Handle("Failed to add motorcycle, rolling back transaction", NotificationType.Error);
                return false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                HandleException(ex);
                return false;
            }
        }
    }

    public async Task<bool> Update(Motorcycle motorcycle, string userEmail)
    {
        if (motorcycle == null)
        {
            _notifier.Handle("Motorcycle details cannot be null", NotificationType.Error);
            return false;
        }

        var existingMotorcycle = await _unitOfWork.Motorcycles.GetById(motorcycle.Id);
        if (existingMotorcycle == null)
        {
            _notifier.Handle("Motorcycle not found", NotificationType.Error);
            return false;
        }

        var validator = new MotorcycleValidation(_unitOfWork);
        validator.ConfigureRulesForUpdate(existingMotorcycle);

        var validationResult = await validator.ValidateAsync(motorcycle);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                UpdateMotorcycleDetails(existingMotorcycle, motorcycle, userEmail);

                await _unitOfWork.Motorcycles.Update(existingMotorcycle);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Motorcycle updated successfully");

                    PublishMotorcycleRegisteredEvent(existingMotorcycle);

                    return true;
                }

                await transaction.RollbackAsync();
                _notifier.Handle("Failed to update motorcycle, rolling back transaction", NotificationType.Error);
                return false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                HandleException(ex);
                return false;
            }
        }
    }

    public async Task<bool> SoftDelete(Guid id, string userEmail)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _notifier.Handle("Invalid motorcycle ID", NotificationType.Error);
                return false;
            }

            var motorcycle = await _unitOfWork.Motorcycles.GetById(id);
            if (motorcycle == null)
            {
                _notifier.Handle("Motorcycle not found", NotificationType.Error);
                return false;
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    motorcycle.UpdatedByUser = userEmail;

                    motorcycle.ToggleIsDeleted();
                    await _unitOfWork.Motorcycles.Update(motorcycle);
                    var result = await _unitOfWork.SaveAsync();

                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        _notifier.Handle("Motorcycle soft deleted successfully");
                        return true;
                    }

                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to soft delete motorcycle, rolling back transaction", NotificationType.Error);
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    HandleException(ex);
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return false;
        }
    }

    private void UpdateMotorcycleDetails(Motorcycle existingMotorcycle, Motorcycle updatedMotorcycle, string userEmail)
    {
        existingMotorcycle.Year = updatedMotorcycle.Year;
        existingMotorcycle.Model = updatedMotorcycle.Model;
        existingMotorcycle.Plate = updatedMotorcycle.Plate;
        existingMotorcycle.UpdatedByUser = userEmail;
        existingMotorcycle.Update();
    }

    private void PublishMotorcycleRegisteredEvent(Motorcycle motorcycle)
    {
        var motorcycleRegisteredEvent = new MotorcycleRegistered
        {
            Id = motorcycle.Id,
            Year = motorcycle.Year,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate,
            CreatedAt = motorcycle.CreatedAt,
            CreatedByUser = motorcycle.CreatedByUser,
            UpdatedAt = motorcycle.UpdatedAt,
            UpdatedByUser = motorcycle.UpdatedByUser,
            IsDeleted = motorcycle.IsDeleted
        };
        _messageProducer.PublishAsync(motorcycleRegisteredEvent, "motorcycle_exchange", "motorcycle_routingKey");
    }
}
