using Ambev.DeveloperEvaluation.Application.Customers.Create;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateCustomerHandlerTestData
{
    private static readonly Faker<CreateCustomerCommand> createCustomerFaker = new Faker<CreateCustomerCommand>()
        .RuleFor(c => c.Name, f => f.Name.FullName())
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("+55###########"));

    public static CreateCustomerCommand GenerateValidCommand()
    {
        return createCustomerFaker.Generate();
    }

    public static CreateCustomerCommand GenerateInvalidCommand()
    {
        var command = createCustomerFaker.Generate();
        command.Email = "invalid-email";
        return command;
    }
}