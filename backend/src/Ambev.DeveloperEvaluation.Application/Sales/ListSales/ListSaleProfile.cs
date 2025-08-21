using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.List;

public class ListSalesProfile : Profile
{
    public ListSalesProfile()
    {
        CreateMap<Sale, ListSalesResult>()
            .ForMember(dest => dest.SaleItem, opt => opt.MapFrom(src => src.Items));
    }
}