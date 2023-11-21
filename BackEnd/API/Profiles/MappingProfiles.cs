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
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Office, OfficeDto>().ReverseMap();
        CreateMap<OfficeEmployee, OfficeEmployeeDto>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<Payment, PaymentDto>().ReverseMap();
        CreateMap<PaymentForm, PaymentFormDto>().ReverseMap();
        CreateMap<Phone, PhoneDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductGamma, ProductGammaDto>().ReverseMap();
        CreateMap<State, StateDto>().ReverseMap();
        CreateMap<StatusOrder, StatusOrderDto>().ReverseMap();
        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<TypePerson, TypePersonDto>().ReverseMap();
    }
}