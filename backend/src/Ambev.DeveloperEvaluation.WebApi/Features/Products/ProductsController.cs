using Ambev.DeveloperEvaluation.Application.Products.Create;
using Ambev.DeveloperEvaluation.Application.Products.Update;
using Ambev.DeveloperEvaluation.Application.Products.Get;
using Ambev.DeveloperEvaluation.Application.Products.List;
using Ambev.DeveloperEvaluation.Application.Products.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Update;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Create;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new ListProductsQuery
        {
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var validator = new GetProductValidator();
        var result = await validator.ValidateAsync(new GetProductCommand(id), cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var product = await _mediator.Send(new GetProductCommand(id), cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductApiRequest req, CancellationToken cancellationToken)
    {
        var validator = new CreateProductApiValidator();
        var result = await validator.ValidateAsync(req, cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var product = await _mediator.Send(new CreateProductCommand() { Name = req.Name, Price = req.Price }, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductApiRequest req, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductApiValidator();
        var result = await validator.ValidateAsync(req, cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var product = await _mediator.Send(new UpdateProductCommand(id, req.Name, req.Price), cancellationToken);
        return product ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if(id == Guid.Empty) return NotFound();

        var product = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return product.Success ? NoContent() : NotFound();
    }
}
