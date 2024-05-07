using AutoMapper;
using BikeRentalSystem.Api.Dtos;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Api.Configurations;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<Courier, CourierDto>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleDto>().ReverseMap();
        CreateMap<Rental, RentalDto>().ReverseMap();
    }
}
