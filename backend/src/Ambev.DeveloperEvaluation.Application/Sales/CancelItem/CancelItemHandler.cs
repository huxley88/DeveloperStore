
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public class CancelItemHandler : IRequestHandler<CancelItemCommand, bool>
{
    private readonly ISaleRepository _cancelItemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelItemHandler> _logger;

    public CancelItemHandler(
    ISaleRepository cancelItemRepository,
    IMapper mapper,
    ILogger<CancelItemHandler> logger)
    {
        _cancelItemRepository = cancelItemRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<bool> Handle(CancelItemCommand request, CancellationToken cancellationToken)
    {
        if(request.SaleId == Guid.Empty) return false;

        var sale = await _cancelItemRepository.GetAsync(request.SaleId, cancellationToken);
        if (sale == null) return false;
        var item = sale.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (item == null) return false;
        item.Cancelled = true;
        sale.RecalculateTotal();
        await _cancelItemRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation("Event: {evt} - SaleNumber={saleNumber}, ProductId={productId}", nameof(ItemCancelled), sale.SaleNumber, request.ProductId);

        return true;
    }
}
