namespace Ambev.DeveloperEvaluation.Application.Sales.Get;

public record GetSaleResult()
{
    public string Id { get; set; }
    public string SaleNumber { get; set; }
    public string CustomerName { get; set; }
    public string BranchName { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }
    public List<GetSaleItem> Items { get; set; }
};

public record GetSaleItem()
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }
};
