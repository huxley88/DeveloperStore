using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.Create
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCustomerHandler> _logger;

        public CreateCustomerHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            ILogger<CreateCustomerHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);

            customer = await _customerRepository.AddAsync(customer, cancellationToken);

            _logger.LogInformation("Event: {evt} - CustomerId={customerId}", nameof(CustomerCreated), customer.Id);

            return _mapper.Map<CreateCustomerResult>(customer);
        }
    }
}
