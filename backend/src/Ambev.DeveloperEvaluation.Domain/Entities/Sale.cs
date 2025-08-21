using System.ComponentModel.DataAnnotations.Schema;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public bool Cancelled { get; set; } = false;
    public decimal TotalAmount { get; private set; }
    public Customer Customer { get; set; }
    private List<SaleItem> _items = new();

    [NotMapped]
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public void InitializeItems(IEnumerable<SaleItem> items)
    {
        _items.Clear();
        _items.AddRange(items);
    }

    public Sale() { }

    public void AddItem(SaleItem item)
    {
        _items.Add(item);
        RecalculateTotal();
    }

    public void UpdateItem(SaleItem item)
    {
        var idx = _items.FindIndex(i => i.Id == item.Id);
        if (idx >= 0) _items[idx] = item;
        RecalculateTotal();
    }

    public void RemoveItem(Guid itemId)
    {
        var it = _items.FirstOrDefault(i => i.Id == itemId);
        if (it != null) _items.Remove(it);
        RecalculateTotal();
    }

    public void RecalculateTotal()
    {
        TotalAmount = _items.Where(i => !i.Cancelled).Sum(i => i.TotalAmount);
    }    
}
