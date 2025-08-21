using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.Create;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CustomProfile : Profile
{
    public CustomProfile()
    {
        CreateMap<CreateCustomerApiRequest, CreateCustomerCommand>();
        CreateMap<CreateCustomerResult, CreateCustomerApiResponse>();
    }
}
