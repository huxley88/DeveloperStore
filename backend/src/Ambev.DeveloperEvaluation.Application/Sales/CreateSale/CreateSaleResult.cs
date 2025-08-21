namespace Ambev.DeveloperEvaluation.Application.Sales.Create;

public class CreateSaleResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string BranchId { get; set; }
    public string BranchName { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemModel> Items { get; set; } = new();

    public CreateSaleResult() { }
}