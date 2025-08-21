using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.List;

public record class ListProductsQuery : IRequest<PagedResult<ListProductsItem>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ListProductsItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public ListProductsItem() { }

    public ListProductsItem(string id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}