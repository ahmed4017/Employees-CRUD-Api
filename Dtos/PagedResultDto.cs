namespace Employees.Dtos
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
