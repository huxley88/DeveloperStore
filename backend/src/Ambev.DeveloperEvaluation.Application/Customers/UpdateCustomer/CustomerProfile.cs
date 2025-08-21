using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.Update;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<UpdateCustomerCommand, Customer>();
    }
}