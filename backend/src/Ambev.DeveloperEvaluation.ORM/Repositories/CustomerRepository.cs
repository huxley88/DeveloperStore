using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;
    public CustomerRepository(DefaultContext ctx) => _context = ctx;

    public async Task<Customer> AddAsync(Customer c, CancellationToken cancellationToken)
    {
        _context.Customers.Add(c);
        await _context.SaveChangesAsync(cancellationToken);
        return c;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var c = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (c is null) return false;
        _context.Customers.Remove(c);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public Task<Customer?> GetAsync(Guid id, CancellationToken ct) =>
        _context.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Customer>> ListAsync(int page, int pageSize, CancellationToken ct) =>
        await _context.Customers.OrderBy(x => x.Name).Skip((page-1)*pageSize).Take(pageSize).ToListAsync(ct);

    public Task<int> CountAsync(CancellationToken ct) => _context.Customers.CountAsync(ct);

    public async Task<bool> UpdateAsync(Customer c, CancellationToken ct)
    {
        _context.Customers.Update(c);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
