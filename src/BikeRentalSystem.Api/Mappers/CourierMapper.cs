using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Api.Models.Request;
using BikeRentalSystem.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace BikeRentalSystem.Api.Mappers;

[ExcludeFromCodeCoverage]
public class CourierMapper : Profile
{
    public CourierMapper()
    {
        CreateMap<CourierDto, CourierRequest>().ReverseMap();
        CreateMap<Courier, CourierRequest>().ReverseMap();
        CreateMap<Courier, CourierDto>().ReverseMap();
    }
}
