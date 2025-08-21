using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer c, CancellationToken cancellationToken);
    Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Customer>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Customer c, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
