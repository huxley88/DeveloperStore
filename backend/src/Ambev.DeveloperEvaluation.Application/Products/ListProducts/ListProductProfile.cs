using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.List;

public class ListProductsProfile : Profile
{
    public ListProductsProfile()
    {
        CreateMap<Product, ListProductsItem>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
}