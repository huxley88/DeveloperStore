using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.List;

public class ListSalesHandler : IRequestHandler<ListSalesQuery, PagedResult<ListSalesResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListSalesHandler> _logger;

    public ListSalesHandler(
    ISaleRepository saleRepository,
    IMapper mapper,
    ILogger<ListSalesHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<PagedResult<ListSalesResult>> Handle(ListSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.ListAsync(request.Page, request.PageSize, cancellationToken);
        var total = await _saleRepository.CountAsync(cancellationToken);

        var mapped = _mapper.Map<List<ListSalesResult>>(sales);

        return new PagedResult<ListSalesResult>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Total = total,
            Data = mapped
        };
    }
}
