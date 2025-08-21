using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Create;

public record class CreateProductCommand : IRequest<CreateProductResult>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }


    public ValidationResultDetail Validate()
    {
        var validator = new CreateProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}