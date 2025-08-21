using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Create;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductApiRequest, CreateProductCommand>();
        CreateMap<CreateProductResult, CreateProductApiResponse>();
    }
}
