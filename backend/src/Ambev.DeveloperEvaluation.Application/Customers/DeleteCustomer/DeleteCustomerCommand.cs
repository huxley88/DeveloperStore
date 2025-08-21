using MediatR;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

namespace Ambev.DeveloperEvaluation.Application.Customers.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<DeleteCustomerResponse>;
