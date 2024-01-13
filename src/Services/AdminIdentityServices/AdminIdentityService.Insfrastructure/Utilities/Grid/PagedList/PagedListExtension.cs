using DDDTemplateServices.Insfrastructure.Utilities.Grid.PagedList;
using Microsoft.EntityFrameworkCore;
namespace AdminIdentityService.Insfrastructure.Utilities.Grid.PagedList
{
    public static class PagedListExtension
    {
        /// <summary>
        /// return to grid property veriable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source,
            int pageIndex,
            int pageSize)
        {
            if (source == null)
                return new PagedList<T>();
            var result = new PagedList<T>
            {
                PageIndex = pageIndex,
                PageSize = Math.Max(pageSize, 1),
                TotalCount = source.Count()
            };
            result.TotalPages = result.TotalCount / result.PageSize;
            result.Data = result.PageSize < result.TotalCount
                ? await source.Skip((result.PageIndex - 1) * result.PageSize)
                    .Take(pageSize)
                    .ToListAsync()
                : await source.ToListAsync();
            if (result.TotalCount % pageSize > 0)
                result.TotalPages++;
            var sourceType = source.ElementType;
            result.PropertyInfos = sourceType.GetProperties().Select(x =>
            {
                var data = new GridPropertyInfo
                {
                    PropertyType = x.PropertyType.Name,
                    PropertyName = char.ToLowerInvariant(x.Name[0]) + x.Name[1..]
                };
                return data;
            });
            return result;
        }
        /// <summary>
        /// if we want to select the pagedlist value
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IPagedList<TResult> Select<TSource, TResult>(this IPagedList<TSource> source,
            Func<TSource, TResult> selector)
        {
            var subset = source.Data.Select(selector);
            IEnumerable<GridPropertyInfo> propertyInfoList = new List<GridPropertyInfo>();
            if (source.Data.GetType() != selector.Method.ReturnType)
            {
                propertyInfoList = selector.Method.ReturnType.GetProperties()
                    .Select(x =>
                    {
                        var data = new GridPropertyInfo
                        {
                            PropertyType = x.PropertyType.Name,
                            PropertyName = char.ToLowerInvariant(x.Name[0]) + x.Name[1..]
                        };
                        return data;
                    });
            }
            else
            {
                propertyInfoList = source.PropertyInfos;
            }
            var result = new PagedList<TResult>(
                data: subset.ToList(),
                pageIndex: source.PageIndex,
                pageSize: source.PageSize,
                totalCount: source.TotalCount,
                totalPages: source.TotalPages,
                propertyInfos: propertyInfoList
                );
            return result;
        }
    }
}
