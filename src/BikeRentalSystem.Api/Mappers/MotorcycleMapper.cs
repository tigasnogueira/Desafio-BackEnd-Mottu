using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Api.Models.Request;
using BikeRentalSystem.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace BikeRentalSystem.Api.Mappers;

[ExcludeFromCodeCoverage]
public class MotorcycleMapper : Profile
{
    public MotorcycleMapper()
    {
        CreateMap<MotorcycleDto, MotorcycleRequest>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleRequest>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleDto>().ReverseMap();
    }
}
