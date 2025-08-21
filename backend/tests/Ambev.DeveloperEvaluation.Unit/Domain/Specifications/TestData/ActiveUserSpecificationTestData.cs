using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData;


public static class ActiveUserSpecificationTestData
{

    private static readonly Faker<User> userFaker = new Faker<User>()
        .CustomInstantiator(f => new User {
            Email = f.Internet.Email(),
            Password = $"Test@{f.Random.Number(100, 999)}",
            Username = f.Name.FirstName(),
            Status = f.PickRandom<UserStatus>(),
            Phone = $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}",
            Role = f.PickRandom<UserRole> ()
        });

     public static User GenerateUser(UserStatus status)
    {
        var user = userFaker.Generate();
        user.Status = status;
        return user;
    }
}
