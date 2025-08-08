namespace Domain.Dtos
{
    public abstract class QueryParamsBase
    {
        public int? ItemsPerPage { get; set; } = 10;
        public int? CurrentPage { get; set; } = 1;
    }
}
