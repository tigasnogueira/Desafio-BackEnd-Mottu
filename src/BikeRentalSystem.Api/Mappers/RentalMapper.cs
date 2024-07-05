using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Api.Models.Request;
using BikeRentalSystem.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace BikeRentalSystem.Api.Mappers;

[ExcludeFromCodeCoverage]
public class RentalMapper : Profile
{
    public RentalMapper()
    {
        CreateMap<RentalDto, RentalRequest>().ReverseMap();
        CreateMap<Rental, RentalRequest>().ReverseMap();
        CreateMap<Rental, RentalDto>()
            .ForMember(dest => dest.Courier, opt => opt.MapFrom(src => src.Courier))
            .ForMember(dest => dest.Motorcycle, opt => opt.MapFrom(src => src.Motorcycle))
            .ReverseMap();
    }
}
