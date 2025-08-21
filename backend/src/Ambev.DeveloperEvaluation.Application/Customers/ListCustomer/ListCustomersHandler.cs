using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.List;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.List
{
    public class ListCustomersHandler : IRequestHandler<ListCustomersQuery, PagedResult<ListCustomersItem>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public ListCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ListCustomersItem>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.ListAsync(request.Page, request.PageSize, cancellationToken);
            var total = await _customerRepository.CountAsync(cancellationToken);

            var items = _mapper.Map<List<ListCustomersItem>>(customers);

            return new PagedResult<ListCustomersItem>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                Data = items
            };
        }
    }
}
