using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(IProductRepository repo, IMapper mapper, ILogger<UpdateProductHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetAsync(request.Id, cancellationToken);
            if (product == null)
                return false;

            _mapper.Map(request, product);

            var success = await _repo.UpdateAsync(product, cancellationToken);

            if (success)
                _logger.LogInformation("Product updated successfully: {productId}", product.Id);
            else
                _logger.LogWarning("Failed to update product: {productId}", product.Id);

            return success;
        }
    }
}
