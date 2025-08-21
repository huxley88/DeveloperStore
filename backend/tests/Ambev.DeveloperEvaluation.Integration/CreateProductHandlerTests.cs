using Ambev.DeveloperEvaluation.Application.Products.Create;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.Create;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateProductHandler>>();
        _handler = new CreateProductHandler(_productRepository, _mapper, _logger);
    }

    [Fact]
    public async Task Handle_Creates_Product_Successfully()
    {
        var cmd = new CreateProductCommand
        {
            Name = "Test Product",
            Price = 10M,
        };

        var productId = Guid.NewGuid();
        var product = new Product
        {
            Name = cmd.Name,
            Price = cmd.Price,
        };

        typeof(Product).GetProperty("Id")?.SetValue(product, productId);

        var productResult = new CreateProductResult
        {
            Id = productId,
            Name = cmd.Name,
            Price = cmd.Price
        };

        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<CreateProductHandler>>();

        mapper.Map<Product>(cmd).Returns(product);
        mapper.Map<CreateProductResult>(product).Returns(productResult);

        _productRepository.AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(product));

        var handler = new CreateProductHandler(_productRepository, mapper, logger);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(productId, result.Id);

        await _productRepository.Received(1).AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}