using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Create;

public class CreateProductApiValidator : AbstractValidator<CreateProductApiRequest>
{
    public CreateProductApiValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
