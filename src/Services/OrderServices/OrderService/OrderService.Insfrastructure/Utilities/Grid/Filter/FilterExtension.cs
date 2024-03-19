using OrderService.Insfrastructure.Utilities.Grid.PagedList;
using System.ComponentModel;
using System.Linq.Expressions;

namespace OrderService.Insfrastructure.Utilities.Grid.Filter
{
    public static class FilterExtension
    {
        /// <summary>
        /// filter expression for dynamic grid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contex"></param>
        /// <param name="filtermodel"></param>
        /// <returns></returns>
#nullable disable
        public static IQueryable<T> ApplyTableFilter<T>(this IQueryable<T> contex,
            PagedListFilterModel filtermodel)
        {
            Expression finalExpression = Expression.Constant(true);
            var parameter = Expression.Parameter(typeof(T), "x");
            if (filtermodel.FilterModelList?.Count > 0)
            {
                foreach (var item in filtermodel.FilterModelList)
                {
                    Expression expression = null;
                    var propertyInfo = contex.GetType().GetGenericArguments()[0].GetProperty(item.PropertyName);
                    var member = Expression.Property(parameter, item.PropertyName);
                    object value = TypeDescriptor.GetConverter(member.Type).ConvertFromString(item.Filter) ?? new object();
                    var constant = Expression.Constant(value, member.Type);
                    switch (Int32.Parse(item.FilterType))
                    {
                        case (int)FilterOperators.Equals:
                            expression = Expression.Equal(member, constant);
                            break;
                        case (int)FilterOperators.Contains:
                            var method = typeof(string).GetMethod("Contains", [typeof(string)]);
                            expression = Expression.Call(member, method, constant);
                            break;
                        case (int)FilterOperators.GreaterThan:
                            expression = Expression.GreaterThanOrEqual(member, constant);
                            break;
                        case (int)FilterOperators.LessThan:
                            expression = Expression.LessThanOrEqual(member, constant);
                            break;
                        case (int)FilterOperators.NotEquals:
                            expression = Expression.NotEqual(member, constant);
                            break;
                    }
                    if (expression != null)
                    {
                        if (item.AndOrOperation == "Or")
                        {
                            finalExpression = Expression.Or(finalExpression, expression);
                        }
                        else
                        {
                            finalExpression = Expression.AndAlso(finalExpression, expression);
                        }
                    }
                }
                var data = contex.Where(Expression.Lambda<Func<T, bool>>(finalExpression, parameter));
                return data;
            }
            else
            {
                return contex;
            }
        }
    }
}