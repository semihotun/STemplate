using CatalogService.Insfrastructure.Utilities.Grid.PagedList;

namespace CatalogService.Insfrastructure.Utilities.Grid.Filter
{
    /// <summary>
    /// all setting expression for dynamic grid
    /// </summary>
    public static class TableExtension
    {
        public static async Task<IPagedList<T>> ToTableSettings<T>(this IQueryable<T> query,
            PagedListFilterModel pagedListFilterModel)
        {
            return await query.ApplyTableFilter(pagedListFilterModel)
                    .ToTableOrderBy(pagedListFilterModel.OrderByColumnName)
                    .ToPagedListAsync(pagedListFilterModel.PageIndex, pagedListFilterModel.PageSize);
        }
    }
}
