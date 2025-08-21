using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Customers.Create;

namespace Ambev.DeveloperEvaluation.Application.Customers.Get;

public class GetCustomerProfile : Profile
{
    public GetCustomerProfile()
    {
        CreateMap<Customer, CreateCustomerResult>();
    }
}