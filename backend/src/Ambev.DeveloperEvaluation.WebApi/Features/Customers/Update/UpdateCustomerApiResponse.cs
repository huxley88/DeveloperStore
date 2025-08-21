namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.Update;

public class UpdateCustomerApiResponse
{
    public Guid id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
