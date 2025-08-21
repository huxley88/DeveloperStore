namespace Ambev.DeveloperEvaluation.Application.Common
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public PagedResult() { }

        public PagedResult(int page, int pageSize, int total, IReadOnlyList<T> data)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            Data = data;
        }
    }
}
