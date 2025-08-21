using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Create;

public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.CustomerName).NotEmpty();
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId).NotEmpty();
            items.RuleFor(i => i.ProductName).NotEmpty();
            items.RuleFor(i => i.Quantity).GreaterThan(0).LessThanOrEqualTo(20);
            items.RuleFor(i => i.UnitPrice).GreaterThan(0);
        });
    }
}
