using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 排序扩展
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 指定属性名，按升序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName">属性或者字段名</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
        }
        /// <summary>
        /// 指定属性名，按降序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName">属性或者字段名</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
        }
        /// <summary>
        /// 指定属性名，按升序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="ordering"><see cref="Ordering"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, Ordering ordering)
        {
            if (ordering == null)
            {
                throw new ArgumentNullException(nameof(ordering));
            }
            string methodName = ordering.Direction == Direction.Asc ? "OrderBy" : "OrderByDescending";
            return CallOrderedQueryable(query, methodName, ordering.Name, null);
        }
        /// <summary>
        /// 指定属性名，按升序对序列中的元素执行后续排序。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName">属性或者字段名</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenBy", propertyName, comparer);
        }
        /// <summary>
        /// 指定属性名，按降序对序列中的元素执行后续排序。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName">属性或者字段名</param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenByDescending", propertyName, comparer);
        }

        /// <summary>
        /// 指定属性名，按升序对序列中的元素执行后续排序。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="ordering"><see cref="Ordering"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, Ordering ordering)
        {
            if (ordering == null)
            {
                throw new ArgumentNullException(nameof(ordering));
            }
            string methodName = ordering.Direction == Direction.Asc ? "ThenBy" : "ThenByDescending";
            return CallOrderedQueryable(query, methodName, ordering.Name, null);
        }

        /// <summary>
        /// 执行多重排序
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="orders">排序信息</param>
        /// <returns>排序结果</returns>
        public static IOrderedQueryable<T> OrderAndThenBy<T>(this IQueryable<T> query, IEnumerable<Ordering> orders)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }
            List<Ordering> orderList = new List<Ordering>(orders);
            if (!orderList.Any())
            {
                throw new ArgumentException(nameof(orders) + ": is empty.");
            }
            var orderedQuery = OrderBy(query, orderList[0]);
            int count = orderList.Count;
            for (int i = 1; i < count; i++)
            {
                orderedQuery = ThenBy(orderedQuery, orderList[i]);
            }
            return orderedQuery;
        }

        /// <summary>
        /// 
        /// 调用排序查询，在(Entity Framework, Linq to Sql)中，不应该使用比较器
        /// </summary>
        public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName, IComparer<object> comparer = null)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer != null
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                    )
                )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                    )
                );
        }
    }
}
