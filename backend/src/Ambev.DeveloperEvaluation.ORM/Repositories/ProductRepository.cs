using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;
    public ProductRepository(DefaultContext ctx) => _context = ctx;

    public async Task<Product> AddAsync(Product p, CancellationToken ct)
    {
        _context.Products.Add(p);
        await _context.SaveChangesAsync(ct);
        return p;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var p = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (p is null) return false;
        _context.Products.Remove(p);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public Task<Product?> GetAsync(Guid id, CancellationToken ct) =>
        _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Product>> ListAsync(int page, int pageSize, CancellationToken ct) =>
        await _context.Products.OrderBy(x => x.Name).Skip((page-1)*pageSize).Take(pageSize).ToListAsync(ct);

    public Task<int> CountAsync(CancellationToken ct) => _context.Products.CountAsync(ct);

    public async Task<bool> UpdateAsync(Product p, CancellationToken ct)
    {
        _context.Products.Update(p);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
