using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.RentalServices.Services;

public class CourierService(ICourierRepository _courierRepository, INotifier _notifier) : BaseService(_notifier), ICourierService
{
    public async Task<Courier> GetById(Guid id)
    {
        try
        {
            _notifier.Handle("Getting courier by ID");
            return await _courierRepository.GetById(id);
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
            return await _courierRepository.GetAll();
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
            return await _courierRepository.GetByCnpj(cnpj);
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
            return await _courierRepository.GetByCnhNumber(cnhNumber);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task Add(Courier courier)
    {
        try
        {
            _notifier.Handle("Adding courier");
            await _courierRepository.Add(courier);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task Update(Courier courier)
    {
        try
        {
            _notifier.Handle("Updating courier");
            await _courierRepository.Update(courier, 0);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task SoftDelete(Guid id)
    {
        try
        {
            _notifier.Handle("Soft deleting courier");
            var courier = await _courierRepository.GetById(id);
            if (courier != null)
            {
                courier.IsDeletedToggle();
                await _courierRepository.Update(courier, 0);
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }
}
