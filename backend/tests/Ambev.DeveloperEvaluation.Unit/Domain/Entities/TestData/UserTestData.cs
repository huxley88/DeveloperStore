using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;


public static class ProductTestData
{
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(u => u.Name, f => f.Internet.UserName())
        .RuleFor(u => u.Price, f => f.Random.Number(18, 2));

    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    public static int  GenerateValidProductPrice()
    {
        return new Faker().Random.Number(18, 2);
    }

    public static string GenerateValidProductname()
    {
        return new Faker().Internet.UserName();
    }

    public static string GenerateLongProductname()
    {
        return new Faker().Random.String2(51);
    }
}
