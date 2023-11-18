using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<City, CityDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<State, StateDto>().ReverseMap();
        CreateMap<Office, OfficeDto>().ReverseMap();
        CreateMap<Phone, PhoneDto>().ReverseMap();
        CreateMap<Phone, PhoneDto>().ReverseMap();
    }
}