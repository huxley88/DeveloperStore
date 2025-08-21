using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.Create;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
