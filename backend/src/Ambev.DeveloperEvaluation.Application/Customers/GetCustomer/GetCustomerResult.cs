
namespace Ambev.DeveloperEvaluation.Application.Customers.Get;

public record class GetCustomerResult
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;   

}
