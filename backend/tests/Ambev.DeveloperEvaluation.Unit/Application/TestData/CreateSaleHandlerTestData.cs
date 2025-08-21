using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<SaleItemModel> saleItemFaker = new Faker<SaleItemModel>()
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 1000))
        .RuleFor(i => i.DiscountPercent, f => f.Random.Decimal(1, 1000));

    private static readonly Faker<CreateSaleCommand> createSaleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => $"SALE-{f.Random.Number(1000, 9999)}")
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => f.Name.FullName())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => saleItemFaker.Generate(f.Random.Int(1, 5)));

    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleFaker.Generate();
    }

    public static CreateSaleCommand GenerateCommandWithExcessiveQuantity()
    {
        var command = createSaleFaker.Generate();
        command.Items[0].Quantity = 25; 
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithNoItems()
    {
        var command = createSaleFaker.Generate();
        command.Items = new List<SaleItemModel>();
        return command;
    }
}