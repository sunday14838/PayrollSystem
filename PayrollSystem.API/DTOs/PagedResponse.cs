namespace PayrollSystem.API.DTOs
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    }
}
