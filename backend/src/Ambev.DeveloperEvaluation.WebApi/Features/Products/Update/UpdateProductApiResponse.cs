namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Update;
public class UpdateProductApiResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
