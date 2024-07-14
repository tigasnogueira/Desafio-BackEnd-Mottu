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

public class RentalService : BaseService, IRentalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageProducer _messageProducer;

    public RentalService(IUnitOfWork unitOfWork, IMessageProducer messageProducer, INotifier notifier) : base(notifier)
    {
        _unitOfWork = unitOfWork;
        _messageProducer = messageProducer;
    }

    public async Task<Rental> GetById(Guid id)
    {
        try
        {
            _notifier.Handle("Getting rental by ID");
            return await _unitOfWork.Rentals.GetById(id);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetAll()
    {
        try
        {
            _notifier.Handle("Getting all rentals");
            return await _unitOfWork.Rentals.GetAll();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<PaginatedResponse<Rental>> GetAllPaged(int page, int pageSize)
    {
        try
        {
            _notifier.Handle("Getting paged rentals");
            return await _unitOfWork.Rentals.GetAllPaged(page, pageSize);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetByCourierId(Guid courierId)
    {
        try
        {
            _notifier.Handle("Getting rentals by courier ID");
            return await _unitOfWork.Rentals.GetByCourierId(courierId);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetByMotorcycleId(Guid motorcycleId)
    {
        try
        {
            _notifier.Handle("Getting rentals by motorcycle ID");
            return await _unitOfWork.Rentals.GetByMotorcycleId(motorcycleId);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetActiveRentals()
    {
        try
        {
            _notifier.Handle("Getting active rentals");
            return await _unitOfWork.Rentals.GetActiveRentals();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<decimal> CalculateRentalCost(Guid rentalId)
    {
        try
        {
            _notifier.Handle("Calculating rental cost");
            return await _unitOfWork.Rentals.CalculateRentalCost(rentalId);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<bool> Add(Rental rental)
    {
        if (rental == null)
        {
            _notifier.Handle("Rental details cannot be null", NotificationType.Error);
            return false;
        }

        var validator = new RentalValidation(_unitOfWork);
        var validationResult = await validator.ValidateAsync(rental);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                var (motorcycles, couriers) = await GetForeignEntities(rental.MotorcycleId, rental.CourierId);

                rental.Motorcycle = motorcycles;
                rental.Courier = couriers;

                await _unitOfWork.Rentals.Add(rental);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Rental added successfully");

                    AddRentalRegisteredEvent(rental);

                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to add rental, rolling back transaction", NotificationType.Error);
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

    public async Task<bool> Update(Rental rental)
    {
        if (rental == null)
        {
            _notifier.Handle("Rental details cannot be null", NotificationType.Error);
            return false;
        }

        var existingRental = await _unitOfWork.Rentals.GetById(rental.Id);
        if (existingRental == null)
        {
            _notifier.Handle("Rental not found", NotificationType.Error);
            return false;
        }

        var validator = new RentalValidation(_unitOfWork);
        var validationResult = await validator.ValidateAsync(rental);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                UpdateRentalDetails(existingRental, rental);

                _unitOfWork.Rentals.Update(existingRental, 0);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Rental updated successfully");

                    AddRentalRegisteredEvent(existingRental);

                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to update rental, rolling back transaction", NotificationType.Error);
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
                _notifier.Handle("Invalid rental ID", NotificationType.Error);
                return false;
            }

            var rental = await _unitOfWork.Rentals.GetById(id);
            if (rental == null)
            {
                _notifier.Handle("Rental not found", NotificationType.Error);
                return false;
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    rental.IsDeletedToggle();
                    await _unitOfWork.Rentals.Update(rental, 0);
                    var result = await _unitOfWork.SaveAsync();

                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        _notifier.Handle("Rental soft deleted successfully");
                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        _notifier.Handle("Failed to soft delete rental, rolling back transaction", NotificationType.Error);
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

    private async Task<(Motorcycle, Courier)> GetForeignEntities(Guid motorcycleId, Guid courierId)
    {
        var motorcycle = await _unitOfWork.Motorcycles.GetById(motorcycleId);
        var courier = await _unitOfWork.Couriers.GetById(courierId);

        return (motorcycle, courier);
    }

    private void UpdateRentalDetails(Rental existingRental, Rental rental)
    {
        existingRental.CourierId = rental.CourierId;
        existingRental.MotorcycleId = rental.MotorcycleId;
        existingRental.StartDate = rental.StartDate;
        existingRental.EndDate = rental.EndDate;
        existingRental.ExpectedEndDate = rental.ExpectedEndDate;
        existingRental.DailyRate = rental.DailyRate;
        existingRental.TotalCost = rental.TotalCost;
        existingRental.Plan = rental.Plan;
        existingRental.Update();
    }

    private void AddRentalRegisteredEvent(Rental rental)
    {
        var rentalRegisteredEvent = new RentalRegistered
        {
            Id = rental.Id,
            CourierId = rental.CourierId,
            MotorcycleId = rental.MotorcycleId,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            ExpectedEndDate = rental.ExpectedEndDate,
            DailyRate = rental.DailyRate,
            TotalCost = rental.TotalCost,
            Plan = rental.Plan,
            CreatedAt = rental.CreatedAt,
            UpdatedAt = rental.UpdatedAt,
            IsDeleted = rental.IsDeleted
        };
        _messageProducer.Publish(rentalRegisteredEvent, "exchange_name", "routing_key");
    }
}
