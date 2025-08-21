using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;


public static class UserTestData
{

    private static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Admin));


    public static User GenerateValidUser()
    {
        return UserFaker.Generate();
    }

    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }

    public static string GenerateValidPassword()
    {
        return $"Test@{new Faker().Random.Number(100, 999)}";
    }

    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}";
    }


    public static string GenerateValidUsername()
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


    public static string GenerateLongUsername()
    {
        return new Faker().Random.String2(51);
    }
}
