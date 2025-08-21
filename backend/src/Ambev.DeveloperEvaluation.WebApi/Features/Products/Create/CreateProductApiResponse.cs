namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Create;

public class CreateProductApiResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
