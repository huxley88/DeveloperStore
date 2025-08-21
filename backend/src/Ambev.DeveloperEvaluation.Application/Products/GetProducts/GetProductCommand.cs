using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Get;

public record GetProductCommand : IRequest<GetProductResult>
{
    public Guid Id { get; }

    public GetProductCommand(Guid id)
    {
        Id = id;
    }
}
