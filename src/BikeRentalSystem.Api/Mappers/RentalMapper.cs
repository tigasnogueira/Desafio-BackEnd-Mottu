﻿using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Api.Models.Request;
using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace BikeRentalSystem.Api.Mappers;

[ExcludeFromCodeCoverage]
public class RentalMapper : Profile
{
    public RentalMapper()
    {
        CreateMap<RentalDto, RentalRequest>().ReverseMap();
        CreateMap<RentalDto, RentalUpdateRequest>().ReverseMap();
        CreateMap<Rental, RentalRequest>().ReverseMap();
        CreateMap<Rental, RentalUpdateRequest>().ReverseMap();
        CreateMap<Rental, RentalDto>()
            .ForMember(dest => dest.Courier, opt => opt.MapFrom(src => src.Courier))
            .ForMember(dest => dest.Motorcycle, opt => opt.MapFrom(src => src.Motorcycle))
            .ReverseMap();

        CreateMap<PaginatedResponse<Rental>, PaginatedResponse<RentalDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
}
