using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;

public record CancelSaleCommand : IRequest<bool>
{
   public string Id {get; set;}
}
