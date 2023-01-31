using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static planGIBusiness.PlanGoodsIssue.SearchDetailModel;

namespace PlanGIBusiness
{
    //public static class QueryableExtensions
    //{
    //    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, Expression<Func<T, object>> keySelector, bool ascending)
    //    {
    //        var selectorBody = keySelector.Body;
    //        // Strip the Convert expression
    //        if (selectorBody.NodeType == ExpressionType.Convert)
    //            selectorBody = ((UnaryExpression)selectorBody).Operand;
    //        // Create dynamic lambda expression
    //        var selector = Expression.Lambda(selectorBody, keySelector.Parameters);
    //        // Generate the corresponding Queryable method call
    //        var queryBody = Expression.Call(typeof(Queryable),
    //            ascending ? "OrderBy" : "OrderByDescending",
    //            new Type[] { typeof(T), selectorBody.Type },
    //            source.Expression, Expression.Quote(selector));
    //        return source.Provider.CreateQuery<T>(queryBody);
    //    }
    //}

    public static class QueryableExtensions
    {
        public static IQueryable<T> KWOrderBy<T>(this IQueryable<T> source, IEnumerable<SortModel> sortModels)
        {
            var expression = source.Expression;
            int count = 0;
            foreach (var item in sortModels)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, item.ColId);
                var method = string.Equals(item.Sort, "desc", StringComparison.OrdinalIgnoreCase) ?
                    (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                    (count == 0 ? "OrderBy" : "ThenBy");
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }


}
