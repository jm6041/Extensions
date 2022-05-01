using Jimlicat.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

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
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object>? comparer = null)
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
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object>? comparer = null)
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
            string methodName = ordering.Dir == Direction.Asc ? "OrderBy" : "OrderByDescending";
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
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object>? comparer = null)
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
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object>? comparer = null)
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
            string methodName = ordering.Dir == Direction.Asc ? "ThenBy" : "ThenByDescending";
            return CallOrderedQueryable(query, methodName, ordering.Name, null);
        }

        /// <summary>
        /// 调用排序查询，在(Entity Framework, Linq to Sql)中，不应该使用比较器
        /// </summary>
        private static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName, IComparer<object>? comparer = null)
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

        /// <summary>
        /// 执行多重排序
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="orders">排序信息</param>
        /// <returns>排序结果</returns>
        private static IOrderedQueryable<T> OrderAndThenBy<T>(this IQueryable<T> query, IEnumerable<Ordering> orders)
        {
            Checker.NotNull(query, nameof(query));
            Checker.NotEmpty(orders, nameof(orders));
            var orderList = ClearNot(typeof(T), orders).ToList();
            var orderedQuery = query.OrderBy(orderList[0]);
            int count = orderList.Count;
            for (int i = 1; i < count; i++)
            {
                orderedQuery = orderedQuery.ThenBy(orderList[i]);
            }
            return orderedQuery;
        }
        private static IEnumerable<Ordering> ClearNot(Type type, IEnumerable<Ordering> orders)
        {
            var pfs = GetPropertiesAndFields(type);
            foreach (var order in orders)
            {
                if (pfs.Any(x => x.Equals(order.Name, StringComparison.OrdinalIgnoreCase)
                || order.Name.StartsWith(x + ".", StringComparison.OrdinalIgnoreCase)))
                {
                    yield return order;
                }
            }
        }
        private static ConcurrentDictionary<Type, string[]> TypeCache = new ConcurrentDictionary<Type, string[]>();
        private static string[] GetPropertiesAndFields(Type type)
        {
            bool s = TypeCache.TryGetValue(type, out string[]? pfs);
            if (!s || pfs == null)
            {
                var ps = type.GetRuntimeProperties().Where(x => x.CanRead).Select(x => x.Name);
                var fs = type.GetRuntimeFields().Where(x => x.IsPublic).Select(x => x.Name);
                List<string> list = new List<string>();
                list.AddRange(ps);
                list.AddRange(fs);
                pfs = list.ToArray();
                TypeCache.TryAdd(type, pfs);
            }
            return pfs;
        }

        /// <summary>
        /// 执行分页, 自动排序
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="page">分页参数</param>
        /// <returns>排序分页后的数据</returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, PageParameter page)
        {
            Checker.NotNull(query, nameof(query));
            Checker.CheckPageParameter(page, nameof(page));
            // 先排序
            if (page.Orderings != null && page.Orderings.Any())
            {
                query = query.OrderAndThenBy(page.Orderings);
            }
            // 后分页
            query = query.Skip(page.PageIndex * page.PageSize).Take(page.PageSize);
            return query;
        }

        /// <summary>
        /// 从全量数据生成分页数据
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="page">分页数据</param>
        /// <returns>分页后的数据</returns>
        public static DataResult<T> ToPagedResult<T>(this IQueryable<T> source, PageParameter page)
        {
            Checker.NotNull(source, nameof(source));
            Checker.CheckPageParameter(page, nameof(page));
            int count = source.Count();
            IList<T> result;
            if (page == null)
            {
                result = source.ToList();
            }
            else
            {
                result = source.Page(page).ToList();
            }
            return new DataResult<T>() { Count = count, Result = result, };
        }

        /// <summary>
        /// OData查询，支持 top, skip, orderby
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="top">加载的数据量</param>
        /// <param name="skip">跳过的数据量</param>
        /// <param name="orderby">排序信息</param>
        /// <returns>排序分页后的数据</returns>
        public static IQueryable<T> ODataQuery<T>(this IQueryable<T> query, int top, int skip, string orderby)
        {
            IReadOnlyDictionary<string, Direction> orderings = ODataParameter.ToOrderingDictionary(orderby);
            return ODataQuery(query, top, skip, orderings);
        }
        /// <summary>
        /// OData查询，支持 top, skip, orderby
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="para">OData参数</param>
        /// <returns></returns>
        public static IQueryable<T> ODataQuery<T>(this IQueryable<T> query, IODataParameter para)
        {
            Checker.NotNull(query, nameof(query));
            Checker.CheckODataParameter(para, nameof(para));
            var orders = para.GetOrderings();
            return ODataQuery(query, para.Top, para.Skip, orders);
        }

        /// <summary>
        /// OData查询
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="top">加载的数据量</param>
        /// <param name="skip">跳过的数据量</param>
        /// <param name="orderings">排序信息</param>
        /// <returns>排序分页后的数据</returns>
        private static IQueryable<T> ODataQuery<T>(IQueryable<T> query, int top, int skip, IReadOnlyDictionary<string, Direction> orderings)
        {
            Checker.NotNull(query, nameof(query));
            Checker.CheckMustNonNegativeInteger(top, nameof(top));
            Checker.CheckMustNonNegativeInteger(skip, nameof(skip));
            // 先排序
            if (orderings != null && orderings.Any())
            {
                var ods = orderings.Select(x => new Ordering { Name = x.Key, Dir = x.Value }).ToArray();
                query = query.OrderAndThenBy(ods);
            }
            // 后缩小查询范围
            if (skip != 0)
            {
                query = query.Skip(skip);
            }
            if (top != int.MaxValue)
            {
                query = query.Take(top);
            }
            return query;
        }

        /// <summary>
        /// 从全量数据生成分页数据
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="para">分页数据</param>
        /// <returns>分页后的数据</returns>
        public static DataResult<T> ToODataResult<T>(this IQueryable<T> source, IODataParameter para)
        {
            Checker.NotNull(source, nameof(source));
            Checker.CheckODataParameter(para, nameof(para));
            int count = 0;
            if (para.Count)
            {
                count = source.Count();
            }
            IList<T> result;
            if (para == null)
            {
                result = source.ToList();
            }
            else
            {
                var orders = para.GetOrderings();
                result = ODataQuery(source, para.Top, para.Skip, orders).ToList();
            }
            return new DataResult<T>() { Count = count, Result = result, };
        }
    }
}
