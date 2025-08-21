namespace Ambev.DeveloperEvaluation.Integration.Common.Sales
{
    namespace Ambev.DeveloperEvaluation.Functional.Models
    {
        public class SaleApiResponse
        {
            public string SaleNumber { get; set; } = string.Empty;
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public string BranchId { get; set; } = string.Empty;
            public string BranchName { get; set; } = string.Empty;
            public List<SaleItemResponse> Items { get; set; } = new();
        }

        public class SaleItemResponse
        {
            public Guid ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
            public decimal DiscountPercent { get; set; }
        }

        public class SalesListResponse
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public List<SaleApiResponse> Data { get; set; } = new();
        }
    }
}
