using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.List
{
    public class ListProductsHandler : IRequestHandler<ListProductsQuery, PagedResult<ListProductsItem>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ListProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ListProductsItem>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.ListAsync(request.Page, request.PageSize, cancellationToken);
            var total = await _productRepository.CountAsync(cancellationToken);

            var items = _mapper.Map<List<ListProductsItem>>(products);

            return new PagedResult<ListProductsItem>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                Data = items
            };
        }
    }
}