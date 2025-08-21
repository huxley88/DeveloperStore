using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Update;
public class UpdateProductApiValidator : AbstractValidator<UpdateProductApiRequest>
{
    public UpdateProductApiValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
