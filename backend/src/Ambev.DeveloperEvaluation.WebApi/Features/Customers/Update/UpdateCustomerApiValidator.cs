using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.Update;

public class UpdateCustomerApiValidator : AbstractValidator<UpdateCustomerApiRequest>
{
    public UpdateCustomerApiValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}