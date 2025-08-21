using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken);
    Task<Sale?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Sale?> GetByNumberAsync(string saleNumber, CancellationToken cancellationToken);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
}
