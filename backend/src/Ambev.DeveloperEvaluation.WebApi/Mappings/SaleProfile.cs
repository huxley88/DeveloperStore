using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;
using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<CreateSaleApiRequest, CreateSaleCommand>();
        CreateMap<SaleItemRequest, SaleItemModel>();
        CreateMap<CreateSaleResult, CreateSaleApiResponse>();
    }
}