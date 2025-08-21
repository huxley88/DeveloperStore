using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Update;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Products.Update;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<UpdateProductCommand, Product>();
    }
}
