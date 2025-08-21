using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.Application.Sales.Get;
using Ambev.DeveloperEvaluation.Application.Sales.List;
using Ambev.DeveloperEvaluation.Application.Sales.Cancel;
using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;
using Ambev.DeveloperEvaluation.Application.Products.List;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleApiRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleApiValidator();

        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var mapper = _mapper.Map<CreateSaleCommand>(request);
        var sale = await _mediator.Send(mapper, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = sale.Id }, sale);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var validator = new GetSaleValidator();
        var result = await validator.ValidateAsync(new GetSaleCommand(id), cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var sale = await _mediator.Send(new GetSaleCommand(id), cancellationToken);
        if (sale == null) return NotFound();
        return Ok(sale);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new ListSalesQuery
        {
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return NotFound();

        var command = new CancelSaleCommand
        {
            Id = id.ToString()
        };

        var canceled = await _mediator.Send(command, cancellationToken);
        return canceled ? NoContent() : NotFound();
    }

    [HttpPost("{id}/items/{productId}/cancel")]
    public async Task<IActionResult> CancelItem(Guid id, Guid productId, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty || productId == Guid.Empty)
            return NotFound();

        var command = new CancelItemCommand
        {
            SaleId = id,
            ProductId = productId
        };

        var canceled = await _mediator.Send(command, cancellationToken);
        return canceled ? NoContent() : NotFound();
    }
}