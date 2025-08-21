
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;

public class CreateSaleApiValidator : AbstractValidator<CreateSaleApiRequest>
{
    public CreateSaleApiValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Items).NotNull();
    }
}
