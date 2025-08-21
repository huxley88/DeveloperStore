namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Create;

public class CreateProductApiRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
