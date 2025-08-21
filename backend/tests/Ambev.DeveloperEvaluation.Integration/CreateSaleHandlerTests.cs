using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Create;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact]
    public async Task Handle_Computes_Discounts_And_Persists()
    {
        var cmd = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "Bob",
            BranchId = "1",
            BranchName = "BH",
            Items = new List<SaleItemModel>
            {
                new() { ProductId = Guid.NewGuid(), ProductName = "Porter 500ml", UnitPrice = 11m, Quantity = 4, DiscountPercent = 10m },
                new() { ProductId = Guid.NewGuid(), ProductName = "Double IPA 330ml", UnitPrice = 14m, Quantity = 1, DiscountPercent = 0m },
                new() { ProductId = Guid.NewGuid(), ProductName = "Pale Ale 330ml", UnitPrice = 9.5m, Quantity = 10, DiscountPercent = 20m },
            }
        };

        var sale = new Sale()
        {
            CustomerId = cmd.CustomerId,
            CustomerName = cmd.CustomerName,
            BranchId = cmd.BranchId,
            BranchName = cmd.BranchName
        };

        foreach (var item in cmd.Items)
        {
            var saleItem = new SaleItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            };
            saleItem.ApplyBusinessRules();
            sale.AddItem(saleItem);
        }

        var saleResult = new CreateSaleResult
        {
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new SaleItemModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercent = i.DiscountPercent
            }).ToList()
        };

        _mapper.Map<Sale>(cmd).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(saleResult);

        _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(sale.TotalAmount, result.TotalAmount);
        Assert.Equal(sale.Items.Count, result.Items.Count);

        await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Creates_Sale_With_Items()
    {
        var cmd = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "Alice",
            BranchId = "2",
            BranchName = "SP",
            Items = new List<SaleItemModel>
            {
                new() { ProductId = Guid.NewGuid(), ProductName = "Weissbier", UnitPrice = 12m, Quantity = 2 },
                new() { ProductId = Guid.NewGuid(), ProductName = "Stout", UnitPrice = 15m, Quantity = 3 }
            }
        };

        var sale = new Sale()
        {
            CustomerId = cmd.CustomerId,
            CustomerName = cmd.CustomerName,
            BranchId = cmd.BranchId,
            BranchName = cmd.BranchName
        };

        foreach (var item in cmd.Items)
        {
            var saleItem = new SaleItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            };
            saleItem.ApplyBusinessRules();
            sale.AddItem(saleItem);
        }

        var saleResult = new CreateSaleResult
        {
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new SaleItemModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercent = i.DiscountPercent
            }).ToList()
        };

        _mapper.Map<Sale>(cmd).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(saleResult);

        _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(sale.TotalAmount, result.TotalAmount);
        Assert.Equal(sale.Items.Count, result.Items.Count);

        await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}