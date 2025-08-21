namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.Create;

public class CreateCustomerApiRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
