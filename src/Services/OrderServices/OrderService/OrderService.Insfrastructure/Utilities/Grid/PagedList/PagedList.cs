namespace OrderService.Insfrastructure.Utilities.Grid.PagedList
{
    /// <summary>
    /// paged list for grid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        public List<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex + 1 < TotalPages;
        public IEnumerable<GridPropertyInfo> PropertyInfos { get; set; }
        public PagedList()
        {
            Data = [];
            PageIndex = 0;
            PageSize = 0;
            TotalCount = 0;
            PropertyInfos = new List<GridPropertyInfo>();
        }
        public PagedList(List<T> data, int pageIndex, int pageSize, int totalCount, int totalPages, IEnumerable<GridPropertyInfo> propertyInfos)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
            PropertyInfos = propertyInfos;
        }
    }
}
