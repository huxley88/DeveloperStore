using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(
    ISaleRepository saleRepository,
    IMapper mapper,
    ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out var id)) return false;

        var sale = await _saleRepository.GetAsync(id, cancellationToken);
        if (sale == null) return false;

        sale.Cancelled = true;
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation("Event: {evt} - SaleNumber={saleNumber}", nameof(SaleCancelled), sale.SaleNumber);

        return true;
    }


}
