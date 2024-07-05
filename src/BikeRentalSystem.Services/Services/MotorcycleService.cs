using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.RentalServices.Services;

public class MotorcycleService(IMotorcycleRepository _motorcycleRepository, INotifier _notifier) : BaseService(_notifier), IMotorcycleService
{
    public async Task<Motorcycle> GetById(Guid id)
    {
        try
        {
            _notifier.Handle("Getting motorcycle by ID");
            return await _motorcycleRepository.GetById(id);
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
            return await _motorcycleRepository.GetAll();
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
            return await _motorcycleRepository.GetByPlate(plate);
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
            return await _motorcycleRepository.GetAllByYear(year);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task Add(Motorcycle motorcycle)
    {
        try
        {
            _notifier.Handle("Adding motorcycle");
            await _motorcycleRepository.Add(motorcycle);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task Update(Motorcycle motorcycle)
    {
        try
        {
            _notifier.Handle("Updating motorcycle");
            await _motorcycleRepository.Update(motorcycle, 0);
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
            _notifier.Handle("Soft deleting motorcycle");
            var motorcycle = await _motorcycleRepository.GetById(id);
            if (motorcycle != null)
            {
                motorcycle.IsDeletedToggle();
                await _motorcycleRepository.Update(motorcycle, 0);
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }
}
