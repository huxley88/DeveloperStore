using Ambev.DeveloperEvaluation.Application.Products.Create;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateProductHandlerTestData
{
    private static readonly Faker<CreateProductCommand> createProductFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000));

    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductFaker.Generate();
    }

    public static CreateProductCommand GenerateInvalidCommand()
    {
        var command = createProductFaker.Generate();
        command.Name = string.Empty; 
        command.Price = -10; 
        return command;
    }
}