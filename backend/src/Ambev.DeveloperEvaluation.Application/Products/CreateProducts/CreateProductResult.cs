namespace Ambev.DeveloperEvaluation.Application.Products.Create
{
    public record class CreateProductResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
