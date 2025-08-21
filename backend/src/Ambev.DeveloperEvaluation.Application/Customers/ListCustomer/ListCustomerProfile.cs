using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Customers.Get;

namespace Ambev.DeveloperEvaluation.Application.Customers.List;

public class ListCustomersProfile : Profile
{
    public ListCustomersProfile()
    {
        CreateMap<Customer, ListCustomersItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<Customer, GetCustomerResult>();
    }
}