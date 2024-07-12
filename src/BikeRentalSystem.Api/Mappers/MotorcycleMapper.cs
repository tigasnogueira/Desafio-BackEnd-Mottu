using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Api.Models.Request;
using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace BikeRentalSystem.Api.Mappers;

[ExcludeFromCodeCoverage]
public class MotorcycleMapper : Profile
{
    public MotorcycleMapper()
    {
        CreateMap<MotorcycleDto, MotorcycleRequest>().ReverseMap();
        CreateMap<MotorcycleDto, MotorcycleUpdateRequest>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleRequest>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleUpdateRequest>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleDto>().ReverseMap();

        CreateMap<PaginatedResponse<Motorcycle>, PaginatedResponse<MotorcycleDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
}
