namespace Ambev.DeveloperEvaluation.Application.Products.Get;

public record class GetProductResult
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
