using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 分页的数据
    /// </summary>
    /// <typeparam name="T">数据源类型</typeparam>
    [Serializable]
    public class PagedResult<T>
    {
        /// <summary>
        /// 分页数据结果
        /// </summary>
        public IList<T> Result { get; set; }
        /// <summary>
        /// 数据总量
        /// </summary>

        public int Toltal { get; set; }
    }

    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// 执行多重排序
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="orders">排序信息</param>
        /// <returns>排序结果</returns>
        private static IOrderedQueryable<T> OrderAndThenBy<T>(this IQueryable<T> query, IEnumerable<Ordering> orders)
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
            var orderedQuery = query.OrderBy(orderList[0]);
            int count = orderList.Count;
            for (int i = 1; i < count; i++)
            {
                orderedQuery = orderedQuery.ThenBy(orderList[i]);
            }
            return orderedQuery;
        }

        /// <summary>
        /// 执行分页, 自动排序，分页
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="query">数据源</param>
        /// <param name="page">分页参数</param>
        /// <returns>排序分页后的数据</returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, PageParameter page)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
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
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, PageParameter page)
        {
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
            return new PagedResult<T>() { Toltal = count, Result = result, };
        }

        /// <summary>
        /// 从全量数据生成分页数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex">加载数据的页号  从0开始</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页后的数据</returns>
        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            int count = source.Count();
            var result = source.Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            return new PagedResult<T>() { Toltal = count, Result = result };
        }
    }
}
