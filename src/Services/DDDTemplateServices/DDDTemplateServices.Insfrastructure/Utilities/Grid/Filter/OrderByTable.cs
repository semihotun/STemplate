using System.Linq.Dynamic.Core;
namespace DDDTemplateServices.Insfrastructure.Utilities.Grid.Filter
{
    /// <summary>
    /// Table order by expression for dynamic grid
    /// </summary>
    public static class OrderByTable
    {
        public static IQueryable<T> ToTableOrderBy<T>(this IQueryable<T> contex,
            string? OrderByColumnName)
        {
            if (!string.IsNullOrEmpty(OrderByColumnName))
                contex = contex.OrderBy(OrderByColumnName.Replace(",", " "));
            return contex;
        }
    }
}
