using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;

namespace BikeRentalSystem.Core.Tests.Helpers;

public static class UnitOfWorkMock
{
    public static IUnitOfWork Create()
    {
        var dataContext = DataContextMock.Create();

        var motorcycleRepository = Substitute.For<IMotorcycleRepository>();
        var courierRepository = Substitute.For<ICourierRepository>();
        var rentalRepository = Substitute.For<IRentalRepository>();

        motorcycleRepository.When(x => x.Add(Arg.Any<Motorcycle>())).Do(x => dataContext.Motorcycles.Add(x.Arg<Motorcycle>()));
        motorcycleRepository.When(x => x.Update(Arg.Any<Motorcycle>())).Do(x => dataContext.Motorcycles.Update(x.Arg<Motorcycle>()));

        courierRepository.When(x => x.Add(Arg.Any<Courier>())).Do(x => dataContext.Couriers.Add(x.Arg<Courier>()));
        courierRepository.When(x => x.Update(Arg.Any<Courier>())).Do(x => dataContext.Couriers.Update(x.Arg<Courier>()));
        courierRepository.GetById(Arg.Any<Guid>()).Returns(id => dataContext.Couriers.Find(id.Arg<Guid>()));
        courierRepository.GetAll().Returns(Task.FromResult((IEnumerable<Courier>)dataContext.Couriers.ToList()));
        courierRepository.GetAllPaged(Arg.Any<int>(), Arg.Any<int>()).Returns(args =>
        {
            int page = args.ArgAt<int>(0);
            int pageSize = args.ArgAt<int>(1);
            var items = dataContext.Couriers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult(new PaginatedResponse<Courier>(items, dataContext.Couriers.Count(), page, pageSize));
        });

        rentalRepository.When(x => x.Add(Arg.Any<Rental>())).Do(x => dataContext.Rentals.Add(x.Arg<Rental>()));
        rentalRepository.When(x => x.Update(Arg.Any<Rental>())).Do(x => dataContext.Rentals.Update(x.Arg<Rental>()));
        rentalRepository.GetById(Arg.Any<Guid>()).Returns(id => dataContext.Rentals.Find(id.Arg<Guid>()));

        var transactionMock = Substitute.For<IDbContextTransaction>();

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.Motorcycles.Returns(motorcycleRepository);
        unitOfWork.Couriers.Returns(courierRepository);
        unitOfWork.Rentals.Returns(rentalRepository);
        unitOfWork.BeginTransactionAsync().Returns(Task.FromResult(transactionMock));
        unitOfWork.SaveAsync().Returns(dataContext.SaveChangesAsync());

        return unitOfWork;
    }
}
