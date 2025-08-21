using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(s => s.SaleNumber, f => $"SALE-{f.Random.Number(1000, 9999)}")
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => f.Random.Number(1, 10).ToString())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Date, f => f.Date.Recent(30))
        .RuleFor(s => s.TotalAmount, f => f.Random.Decimal(50, 1000));

    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(5, 100))
        .RuleFor(i => i.Quantity, f => f.Random.Number(1, 20))
        .RuleFor(i => i.TotalAmount, (f, i) => i.UnitPrice * i.Quantity);

    public static Sale GenerateValidSale()
    {
        var sale = SaleFaker.Generate();

        var itemsCount = new Faker().Random.Number(1, 5);
        var items = SaleItemFaker.Generate(itemsCount);
        sale.InitializeItems(items);

        return sale;
    }

    public static Sale GenerateValidSaleWithItems(int numberOfItems = 3)
    {
        var sale = SaleFaker.Generate();

        var items = SaleItemFaker.Generate(numberOfItems);
        sale.InitializeItems(items);

        return sale;
    }

    public static Sale GenerateSaleWithoutItems()
    {
        var sale = SaleFaker.Generate();
        sale.InitializeItems(new List<SaleItem>());
        return sale;
    }

    public static Sale GenerateSaleWithSpecificStatus()
    {
        var sale = SaleFaker.Generate();

        var statusProperty = sale.GetType().GetProperty("Status");
        if (statusProperty != null && statusProperty.CanWrite)
        {
        }

        var itemsCount = new Faker().Random.Number(1, 3);
        var items = SaleItemFaker.Generate(itemsCount);
        sale.InitializeItems(items);

        return sale;
    }

    public static Sale GenerateSaleWithHighValue()
    {
        var sale = SaleFaker.Generate();

        var highValueItemFaker = new Faker<SaleItem>()
            .RuleFor(i => i.ProductId, f => f.Random.Guid())
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(100, 500))
            .RuleFor(i => i.Quantity, f => f.Random.Number(5, 10));

        var items = highValueItemFaker.Generate(2);
        sale.InitializeItems(items);

        return sale;
    }

    public static List<SaleItem> GenerateSaleItems(int count = 3)
    {
        return SaleItemFaker.Generate(count);
    }

    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}";
    }

    public static string GenerateValidSalename()
    {
        return new Faker().Internet.UserName();
    }

    public static string GenerateInvalidEmail()
    {
        var faker = new Faker();
        return faker.Lorem.Word();
    }

    public static string GenerateInvalidPassword()
    {
        return new Faker().Lorem.Word();
    }

    public static string GenerateInvalidPhone()
    {
        return new Faker().Random.AlphaNumeric(5);
    }

    public static string GenerateLongSalename()
    {
        return new Faker().Random.String2(51);
    }

    public static IEnumerable<Sale> GenerateMultipleSales(int count = 5)
    {
        var sales = SaleFaker.Generate(count);

        foreach (var sale in sales)
        {
            var itemsCount = new Faker().Random.Number(1, 4);
            var items = SaleItemFaker.Generate(itemsCount);
            sale.InitializeItems(items);
        }

        return sales;
    }

    public static IEnumerable<Sale> GenerateMultipleSalesLinq(int count = 5)
    {
        return SaleFaker.Generate(count).Select(sale =>
        {
            var itemsCount = new Faker().Random.Number(1, 4);
            var items = SaleItemFaker.Generate(itemsCount);
            sale.InitializeItems(items);
            return sale;
        }).ToList();
    }
}