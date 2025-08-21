
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Integration.Common;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;

public class SaleRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task Add_And_Get_Sale_Persists_Items_With_Discount()
    {
        var repo = new SaleRepository(Db);

        var sale = new Sale
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "Alice",
            BranchId = "1",
            BranchName = "Main Branch",
        };

        sale.InitializeItems(new List<SaleItem>
        {
            new SaleItem { ProductId = Guid.NewGuid(), ProductName = "Amber Ale", UnitPrice = 7m, Quantity = 4 , DiscountPercent = 10},
            new SaleItem { ProductId = Guid.NewGuid(), ProductName = "Pale Ale", UnitPrice = 9.5m, Quantity = 11, DiscountPercent = 20 },
        });

        await repo.AddAsync(sale, default);
        await Db.SaveChangesAsync();

        var back = await repo.GetAsync(sale.Id, default);
        Assert.NotNull(back);
        Assert.Equal(2, back!.Items.Count);
        Assert.Contains(back.Items, i => i.DiscountPercent == 10m);
        Assert.Contains(back.Items, i => i.DiscountPercent == 20m);
    }
}
