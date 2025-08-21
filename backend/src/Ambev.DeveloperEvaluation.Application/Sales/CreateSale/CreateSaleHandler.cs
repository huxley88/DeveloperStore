using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Application.Sales.Create;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(
    ISaleRepository saleRepository,
    IMapper mapper,
    ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = _mapper.Map<Sale>(request);

        foreach (var item in sale.Items)
        {
            item.ApplyBusinessRules();
        }

        sale.RecalculateTotal();
        sale = await _saleRepository.AddAsync(sale, cancellationToken);

        _logger.LogInformation("Event: {evt} - SaleNumber={saleNumber}", nameof(SaleCreated), sale.SaleNumber);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}
