namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create
{
	public class CreateSaleApiRequest
    {
        public string SaleNumber { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
		public DateTime Date { get; set; }
		public List<SaleItemRequest> Items { get; set; } = new();
	}

	public class SaleItemRequest
	{
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public string ProductName { get; set; }
    }
}
