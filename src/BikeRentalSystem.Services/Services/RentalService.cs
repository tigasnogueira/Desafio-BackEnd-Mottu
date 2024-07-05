using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.RentalServices.Services;

public class RentalService(IRentalRepository _rentalRepository, INotifier _notifier) : BaseService(_notifier), IRentalService
{
    public async Task<Rental> GetById(Guid id)
    {
        try
        {
            _notifier.Handle("Getting rental by ID");
            return await _rentalRepository.GetById(id);
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
            return await _rentalRepository.GetAll();
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
            return await _rentalRepository.GetByCourierId(courierId);
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
            return await _rentalRepository.GetByMotorcycleId(motorcycleId);
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
            return await _rentalRepository.GetActiveRentals();
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
            return await _rentalRepository.CalculateRentalCost(rentalId);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task Add(Rental rental)
    {
        try
        {
            _notifier.Handle("Adding rental");
            await _rentalRepository.Add(rental);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task Update(Rental rental)
    {
        try
        {
            _notifier.Handle("Updating rental");
            await _rentalRepository.Update(rental, 0);
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
            _notifier.Handle("Soft deleting rental");
            var rental = await _rentalRepository.GetById(id);
            if (rental != null)
            {
                rental.IsDeletedToggle();
                await _rentalRepository.Update(rental, 0);
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }
}
