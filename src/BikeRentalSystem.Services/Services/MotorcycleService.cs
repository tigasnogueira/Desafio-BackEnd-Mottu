using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
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
        _unitOfWork = unitOfWork;
        _messageProducer = messageProducer;
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

    public async Task<bool> Add(Motorcycle motorcycle)
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
                await _unitOfWork.Motorcycles.Add(motorcycle);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Motorcycle added successfully");

                    AddMotorcycleRegisteredEvent(motorcycle);

                    if (motorcycle.Year == 2024)
                    {
                        _notifier.Handle("Motorcycle year is 2024, sending notification");
                    }

                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to add motorcycle, rolling back transaction", NotificationType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                HandleException(ex);
                return false;
            }
        }
    }

    public async Task<bool> Update(Motorcycle motorcycle)
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
                UpdateMotorcycleDetails(existingMotorcycle, motorcycle);

                _unitOfWork.Motorcycles.Update(existingMotorcycle);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Motorcycle updated successfully");

                    AddMotorcycleRegisteredEvent(existingMotorcycle);

                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to update motorcycle, rolling back transaction", NotificationType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                HandleException(ex);
                return false;
            }
        }
    }

    public async Task<bool> SoftDelete(Guid id)
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
                    motorcycle.IsDeletedToggle();
                    await _unitOfWork.Motorcycles.Update(motorcycle);
                    var result = await _unitOfWork.SaveAsync();

                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        _notifier.Handle("Motorcycle soft deleted successfully");
                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        _notifier.Handle("Failed to soft delete motorcycle, rolling back transaction", NotificationType.Error);
                        return false;
                    }
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

    private void UpdateMotorcycleDetails(Motorcycle existingMotorcycle, Motorcycle updatedMotorcycle)
    {
        existingMotorcycle.Year = updatedMotorcycle.Year;
        existingMotorcycle.Model = updatedMotorcycle.Model;
        existingMotorcycle.Plate = updatedMotorcycle.Plate;
        existingMotorcycle.Update();
    }

    private void AddMotorcycleRegisteredEvent(Motorcycle motorcycle)
    {
        var motorcycleRegisteredEvent = new MotorcycleRegistered
        {
            Id = motorcycle.Id,
            Year = motorcycle.Year,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate,
            CreatedAt = motorcycle.CreatedAt,
            UpdatedAt = motorcycle.UpdatedAt,
            IsDeleted = motorcycle.IsDeleted
        };
        _messageProducer.Publish(motorcycleRegisteredEvent, "exchange_name", "routing_key");
    }
}
