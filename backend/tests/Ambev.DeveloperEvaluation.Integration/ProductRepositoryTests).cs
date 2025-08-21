
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Integration.Common;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;

public class ProductRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task Add_And_Get_Product_Persists_Items_With_Discount()
    {
        var repo = new ProductRepository(Db);

        var product = new Product
        {
            Name = "teste",
            Price = 11M,
        };

        product.Validate();

        await repo.AddAsync(product, default);
        await Db.SaveChangesAsync();

        var back = await repo.GetAsync(product.Id, default);
        Assert.NotNull(back);
    }
}
