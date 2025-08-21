using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Delete;

public record DeleteProductCommand(Guid Id) : IRequest<DeleteProductResponse>;
