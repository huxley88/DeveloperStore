using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Products.Update;

public record class UpdateProductCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public UpdateProductCommand() { }

    public UpdateProductCommand(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
