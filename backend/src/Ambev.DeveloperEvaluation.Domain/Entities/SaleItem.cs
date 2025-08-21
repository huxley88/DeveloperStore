
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; } // 0, 10, 20
    public decimal TotalAmount { get; private set; }
    public bool Cancelled { get; set; } = false;

    public void ApplyBusinessRules()
    {
        if (Quantity < 0) throw new DomainException("Quantity cannot be negative!");
        if (Quantity > 20) throw new DomainException("It's not possible to sell above 20 identical items!");

        if (Quantity >= 10) DiscountPercent = 20m;
        else if (Quantity >= 4) DiscountPercent = 10m;
        else DiscountPercent = 0m;

        var gross = UnitPrice * Quantity;
        var discount = gross * (DiscountPercent / 100m);
        TotalAmount = Math.Round(gross - discount, 2, MidpointRounding.AwayFromZero);
    }
}
