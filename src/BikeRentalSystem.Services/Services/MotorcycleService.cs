using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Notifications;

namespace BikeRentalSystem.RentalServices.Services;

public class MotorcycleService(IUnitOfWork _unitOfWork, INotifier _notifier) : BaseService(_notifier), IMotorcycleService
{
    public async Task<Motorcycle> GetById(Guid id)
    {
        _motorcycleRepository = motorcycleRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task<Motorcycle> GetMotorcycleByIdAsync(Guid id)
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

    public async Task<Motorcycle> GetByPlate(string plate)
    {
        try
        {
            _notifier.Handle("Getting motorcycle by plate");
            return await _unitOfWork.Motorcycles.GetByPlate(plate);
            }

            return addedMotorcycle;
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
        var validationResult = await validator.ValidateAsync(motorcycle);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }
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
                existingMotorcycle.Identifier = motorcycle.Identifier;
                existingMotorcycle.Year = motorcycle.Year;
                existingMotorcycle.Model = motorcycle.Model;
                existingMotorcycle.Plate = motorcycle.Plate;
                existingMotorcycle.Update();

                _unitOfWork.Motorcycles.Update(existingMotorcycle, 0);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Motorcycle updated successfully");
                    return true;
        }
                else
        {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to update motorcycle, rolling back transaction", NotificationType.Error);
                    return false;
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByYearAsync(int year)
    {
        try
        {
            _notifier.Handle($"Motorcycles by year {year} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByYearAsync(year);
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
                    await _unitOfWork.Motorcycles.Update(motorcycle, 0);
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

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileageAsync(int mileage)
    {
        try
        {
            _notifier.Handle($"Motorcycles by mileage {mileage} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByMileageAsync(mileage);
        }
        catch (Exception ex)
        {
                    await transaction.RollbackAsync();
                    HandleException(ex);
                    return false;
        }
    }

    public async Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate)
    {
        try
        {
            _notifier.Handle($"Motorcycle by license plate {licensePlate} was accessed");
            return await _motorcycleRepository.GetMotorcycleByLicensePlateAsync(licensePlate);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return false;
    }

    public async Task<Motorcycle> UpdateMotorcycleLicensePlateAsync(Guid id, string newLicensePlate)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
            throw new KeyNotFoundException("Motorcycle not found.");

        if (!ExecuteValidation(new MotorcycleValidation(_motorcycleRepository), new Motorcycle { LicensePlate = newLicensePlate }))
            return null;

        motorcycle.LicensePlate = newLicensePlate;
        await _motorcycleRepository.UpdateAsync(motorcycle);
        return motorcycle;
    }
}
