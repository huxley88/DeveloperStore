
using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Tests.Domain;

public class SaleDomainTests
{
    [Fact]
    public void Quantity_4_to_9_Gets_10pct_Discount()
    {
        var item = new SaleItem { ProductId = "P1", ProductName = "Prod", Quantity = 5, UnitPrice = 100 };
        item.ApplyBusinessRules();
        item.DiscountPercent.Should().Be(10m);
        item.TotalAmount.Should().Be(450m);
    }

    [Fact]
    public void Quantity_10_to_20_Gets_20pct_Discount()
    {
        var item = new SaleItem { ProductId = "P2", ProductName = "Prod", Quantity = 12, UnitPrice = 50 };
        item.ApplyBusinessRules();
        item.DiscountPercent.Should().Be(20m);
        item.TotalAmount.Should().Be(480m);
    }

    [Fact]
    public void Quantity_Over_20_Throws()
    {
        var item = new SaleItem { ProductId = "P3", ProductName = "Prod", Quantity = 21, UnitPrice = 10 };
        Action act = () => item.ApplyBusinessRules();
        act.Should().Throw<ArgumentException>();
    }
}
