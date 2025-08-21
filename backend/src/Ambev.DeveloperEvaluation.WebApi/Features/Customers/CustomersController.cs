using Ambev.DeveloperEvaluation.Application.Customers.Create;
using Ambev.DeveloperEvaluation.Application.Customers.Update;
using Ambev.DeveloperEvaluation.Application.Customers.Get;
using Ambev.DeveloperEvaluation.Application.Customers.List;
using Ambev.DeveloperEvaluation.Application.Customers.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.Update;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.WebApi.Common;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : BaseController
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new ListCustomersQuery
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
        var validator = new GetCustomerValidator();
        var result = await validator.ValidateAsync(new GetCustomerCommand(id), cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var customer = await _mediator.Send(new GetCustomerCommand(id), cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerApiRequest req, CancellationToken cancellationToken)
    {
        var validator = new CreateCustomerApiValidator();
        var result = await validator.ValidateAsync(req, cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors);

        var customer = await _mediator.Send(new CreateCustomerCommand() { Name = req.Name, Email = req.Email, Phone = req.Phone }, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerApiRequest req, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerApiValidator();
        var validationResult = await validator.ValidateAsync(req, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var command = new UpdateCustomerCommand
        {
            Id = id,
            Name = req.Name,
            Email = req.Email,
            Phone = req.Phone
        };

        var updated = await _mediator.Send(command, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty) return NotFound();

        var result = await _mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
        return result.Success ? NoContent() : NotFound();
    }
}