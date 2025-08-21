
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;
    public SaleRepository(DefaultContext context) => _context = context;

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellation)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellation);
        return sale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellation)
    {
        var sale = await _context.Sales.Include("_items").FirstOrDefaultAsync(s => s.Id == id, cancellation);
        if (sale == null) return false;
        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellation);
        return true;
    }

    public Task<Sale?> GetAsync(Guid id, CancellationToken cancellation) =>
        _context.Sales.Include("_items").FirstOrDefaultAsync(s => s.Id == id, cancellation);

    public Task<Sale?> GetByNumberAsync(string saleNumber, CancellationToken cancellation) =>
        _context.Sales.Include("_items").FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellation);

    public async Task<IReadOnlyList<Sale>> ListAsync(int page, int pageSize, CancellationToken cancellation) =>
        await _context.Sales.Include("_items")
            .OrderByDescending(s => s.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellation);

    public Task<int> CountAsync(CancellationToken cancellation) => _context.Sales.CountAsync(cancellation);

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellation)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellation);
        return sale;
    }
}
