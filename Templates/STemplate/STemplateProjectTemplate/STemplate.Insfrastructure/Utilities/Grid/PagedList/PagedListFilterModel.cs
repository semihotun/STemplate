using STemplate.Insfrastructure.Utilities.Grid.Filter;
namespace STemplate.Insfrastructure.Utilities.Grid.PagedList
{
    /// <summary>
    /// Pagedlist filter model
    /// </summary>
    public class PagedListFilterModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? OrderByColumnName { get; set; }
        public List<FilterModel>? FilterModelList { get; set; }
    }
}
