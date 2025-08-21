using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Create
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Items, opt => opt.Ignore()) 
                .AfterMap((src, dest, ctx) =>
                {
                    var items = src.Items?.Select(i => ctx.Mapper.Map<SaleItem>(i)).ToList() ?? new List<SaleItem>();
                    dest.InitializeItems(items);
                });

            CreateMap<SaleItemModel, SaleItem>();

            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TotalAmount,
                           opt => opt.MapFrom(src => src.Items.Where(i => !i.Cancelled).Sum(i => i.TotalAmount)));

            CreateMap<SaleItem, SaleItemModel>();
        }
    }
}
