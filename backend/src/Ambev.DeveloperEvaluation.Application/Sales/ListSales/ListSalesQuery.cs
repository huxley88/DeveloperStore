using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.List;

public record ListSalesQuery : IRequest<PagedResult<ListSalesResult>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
public record ListSalesResult
{
    public string Id { get; set; }
    public string SaleNumber { get; set; }
    public string CustomerName { get; set; }
    public string BranchName { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }
    public List<SaleItemModel> SaleItem { get; set; }
}

