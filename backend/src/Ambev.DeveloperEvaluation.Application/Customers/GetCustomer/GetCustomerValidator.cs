using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.Get;

public class GetCustomerValidator : AbstractValidator<GetCustomerCommand>

{ public GetCustomerValidator()
    {
        RuleFor(x=>x.Id)
            .NotEmpty()
            .WithMessage("Customer ID is required"); ; 
    } 
}
