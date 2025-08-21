
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Integration.Common;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;

public class CustomerRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task Add_And_Get_Customer_Persists_Items_With_Discount()
    {
        var repo = new CustomerRepository(Db);

        var customer = new Customer
        {
            Name = "teste",
            Email = "Alice@teste.com",
            Phone = "12888888888",
        };
        customer.Validate();

        await repo.AddAsync(customer, default);
        await Db.SaveChangesAsync();

        var back = await repo.GetAsync(customer.Id, default);
        Assert.NotNull(back);
    }
}
