
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Tests.Application;

public class CreateSaleHandlerTests
{
    [Fact]
    public async Task Should_Create_Sale_With_Items_And_Apply_Rules()
    {
        var repo = new Mock<ISaleRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sale s, CancellationToken _) => s);

        var handler = new CreateSaleHandler(repo.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<CreateSaleHandler>>());

        var cmd = new CreateSaleCommand(
            "S-2000",
            "C1", "Cliente Teste",
            "B1", "Filial Teste",
            new List<SaleItemModel> {
                new() { ProductId = "P1", ProductName = "Produto A", Quantity = 6, UnitPrice = 10m }
            }
        );

        var result = await handler.Handle(cmd, CancellationToken.None);

        result.Should().NotBeNull();
        result.SaleNumber.Should().Be("S-2000");
        result.TotalAmount.Should().Be(54m); // 6 * 10 = 60 with 10% = 54
        repo.Verify(r => r.AddAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
