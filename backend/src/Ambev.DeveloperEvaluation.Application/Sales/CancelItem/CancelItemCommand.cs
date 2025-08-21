using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public record CancelItemCommand : IRequest<bool>
{
    public Guid SaleId { get; init; }
    public Guid ProductId { get; init; }
}
