using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.Create;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{ public CreateProductValidator()
    {
        RuleFor(x=>x.Name).NotEmpty(); 
        RuleFor(x=>x.Price).GreaterThan(0); 
    } 
}
