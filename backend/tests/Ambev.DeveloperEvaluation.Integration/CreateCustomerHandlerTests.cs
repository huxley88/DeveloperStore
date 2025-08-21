using Ambev.DeveloperEvaluation.Application.Customers.Create;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Customers.Create;

public class CreateCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateCustomerHandler>>();
        _handler = new CreateCustomerHandler(_customerRepository, _mapper, _logger);
    }

    [Fact]
    public async Task Handle_Creates_Customer_Successfully()
    {
        var cmd = new CreateCustomerCommand
        {
            Name = "teste",
            Email = "test@gmail.com",
            Phone = "19999999999",
        };

        var customer = new Customer { Name = cmd.Name, Email = cmd.Email, Phone = cmd.Phone };
        var customerResult = new CreateCustomerResult { Name = cmd.Name, Email = cmd.Email, Phone = cmd.Phone };

        _mapper.Map<Customer>(cmd).Returns(customer);
        _mapper.Map<CreateCustomerResult>(customer).Returns(customerResult);

        _customerRepository.AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(customer));

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(cmd.Name, result.Name);
        Assert.Equal(cmd.Email, result.Email);
        Assert.Equal(cmd.Phone, result.Phone);

        await _customerRepository.Received(1).AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Creates_Customer_With_Valid_Data()
    {
        var cmd = new CreateCustomerCommand
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Phone = "+5511999999999",
        };

        var customer = new Customer
        {
            Name = cmd.Name,
            Email = cmd.Email,
            Phone = cmd.Phone
        };

        var customerResult = new CreateCustomerResult
        {
            Name = cmd.Name,
            Email = cmd.Email,
            Phone = cmd.Phone
        };

        var mapper = NSubstitute.Substitute.For<AutoMapper.IMapper>();
        var logger = NSubstitute.Substitute.For<ILogger<CreateCustomerHandler>>();

        mapper.Map<Customer>(cmd).Returns(customer);
        mapper.Map<CreateCustomerResult>(customer).Returns(customerResult);

        _customerRepository.AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(customer));

        var handler = new CreateCustomerHandler(_customerRepository, mapper, logger);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("João Silva", result.Name);
        Assert.Equal("joao.silva@email.com", result.Email);

        await _customerRepository.Received(1).AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }
}