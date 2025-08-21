using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;


public static class CustomerTestData
{

    private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
        .RuleFor(u => u.Name, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}");


    public static Customer GenerateValidCustomer()
    {
        return CustomerFaker.Generate();
    }

    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }


    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}";
    }


    public static string GenerateValidCustomername()
    {
        return new Faker().Internet.UserName();
    }

    public static string GenerateInvalidEmail()
    {
        var faker = new Faker();
        return faker.Lorem.Word();
    }


    public static string GenerateInvalidPhone()
    {
        return new Faker().Random.AlphaNumeric(5);
    }

    public static string GenerateLongCustomername()
    {
        return new Faker().Random.String2(51);
    }
}
