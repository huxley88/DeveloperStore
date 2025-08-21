using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Update;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _repo;
    private readonly IMapper _mapper;

    public UpdateCustomerHandler(ICustomerRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repo.GetAsync(request.Id, cancellationToken);
        if (customer is null) return false;

        _mapper.Map(request, customer);

        return await _repo.UpdateAsync(customer, cancellationToken);
    }
}