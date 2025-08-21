using FluentValidation;
namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.Create;
public class CreateCustomerApiValidator : AbstractValidator<CreateCustomerApiRequest>
{
    public CreateCustomerApiValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
