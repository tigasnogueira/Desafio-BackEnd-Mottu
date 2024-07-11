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

public class CourierService(IUnitOfWork _unitOfWork, IMessageProducer _messageProducer, INotifier _notifier) : BaseService(_notifier), ICourierService
{
    public async Task<Courier> GetById(Guid id)
    {
        try
        {
            _notifier.Handle("Getting courier by ID");
            return await _unitOfWork.Couriers.GetById(id);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetAll()
    {
        try
        {
            _notifier.Handle("Getting all couriers");
            return await _unitOfWork.Couriers.GetAll();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<PaginatedResponse<Courier>> GetAllPaged(int page, int pageSize)
    {
        try
        {
            _notifier.Handle("Getting paged couriers");
            return await _unitOfWork.Couriers.GetAllPaged(page, pageSize);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<Courier> GetByCnpj(string cnpj)
    {
        try
        {
            _notifier.Handle("Getting courier by CNPJ");
            return await _unitOfWork.Couriers.GetByCnpj(cnpj);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<Courier> GetByCnhNumber(string cnhNumber)
    {
        try
        {
            _notifier.Handle("Getting courier by CNH number");
            return await _unitOfWork.Couriers.GetByCnhNumber(cnhNumber);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<bool> Add(Courier courier)
    {
        if (courier == null)
        {
            _notifier.Handle("Courier details cannot be null", NotificationType.Error);
            return false;
        }

        var validator = new CourierValidation(_unitOfWork);
        var validationResult = await validator.ValidateAsync(courier);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                await _unitOfWork.Couriers.Add(courier);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Courier added successfully");

                    var courierRegisteredEvent = new CourierRegistered
                    {
                        Id = courier.Id,
                        Name = courier.Name,
                        Cnpj = courier.Cnpj,
                        BirthDate = courier.BirthDate,
                        CnhNumber = courier.CnhNumber,
                        CnhType = courier.CnhType,
                        CnhImage = courier.CnhImage,
                        CreatedAt = courier.CreatedAt,
                        UpdatedAt = courier.UpdatedAt,
                        IsDeleted = courier.IsDeleted
                    };
                    _messageProducer.Publish(courierRegisteredEvent, "exchange_name", "routing_key");

                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to add courier, rolling back transaction", NotificationType.Error);
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

    public async Task<bool> Update(Courier courier)
    {
        if (courier == null)
        {
            _notifier.Handle("Courier details cannot be null", NotificationType.Error);
            return false;
        }

        var existingCourier = await _unitOfWork.Couriers.GetById(courier.Id);
        if (existingCourier == null)
        {
            _notifier.Handle("Courier not found", NotificationType.Error);
            return false;
        }

        var validator = new CourierValidation(_unitOfWork);
        var validationResult = await validator.ValidateAsync(courier);
        if (!validationResult.IsValid)
        {
            _notifier.NotifyValidationErrors(validationResult);
            return false;
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                existingCourier.Name = courier.Name;
                existingCourier.Cnpj = courier.Cnpj;
                existingCourier.BirthDate = courier.BirthDate;
                existingCourier.CnhNumber = courier.CnhNumber;
                existingCourier.CnhType = courier.CnhType;
                existingCourier.CnhImage = courier.CnhImage;
                existingCourier.Update();

                _unitOfWork.Couriers.Update(existingCourier, 0);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _notifier.Handle("Courier updated successfully");

                    var courierRegisteredEvent = new CourierRegistered
                    {
                        Id = courier.Id,
                        Name = courier.Name,
                        Cnpj = courier.Cnpj,
                        BirthDate = courier.BirthDate,
                        CnhNumber = courier.CnhNumber,
                        CnhType = courier.CnhType,
                        CnhImage = courier.CnhImage,
                        CreatedAt = courier.CreatedAt,
                        UpdatedAt = courier.UpdatedAt,
                        IsDeleted = courier.IsDeleted
                    };
                    _messageProducer.Publish(courierRegisteredEvent, "exchange_name", "routing_key");

                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    _notifier.Handle("Failed to update courier, rolling back transaction", NotificationType.Error);
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
                _notifier.Handle("Invalid courier ID", NotificationType.Error);
                return false;
            }

            var courier = await _unitOfWork.Couriers.GetById(id);
            if (courier == null)
            {
                _notifier.Handle("Courier not found", NotificationType.Error);
                return false;
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    courier.IsDeletedToggle();
                    await _unitOfWork.Couriers.Update(courier, 0);
                    var result = await _unitOfWork.SaveAsync();

                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        _notifier.Handle("Courier soft deleted successfully");
                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        _notifier.Handle("Failed to soft delete courier, rolling back transaction", NotificationType.Error);
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
}
