using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jimlicat.Utils;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 分页异步扩展
    /// </summary>
    public static class PagingAsyncExtension
    {
        /// <summary>
        /// 从全量数据生成分页数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderings">排序信息</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, IDictionary<string, Direction> orderings = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(source, nameof(source));
            Checker.CheckMustNonNegativeInteger(pageIndex, nameof(pageIndex));
            Checker.CheckMustNonNegativeInteger(pageSize, nameof(pageSize));
            int count = await source.CountAsync(cancellationToken);
            if (pageSize <= 0)
            {
                return new PagedResult<T>() { Toltal = count, Result = Enumerable.Empty<T>().ToList(), };
            }
            var result = await source.Page(pageIndex, pageSize, orderings).ToListAsync(cancellationToken);
            return new PagedResult<T>() { Toltal = count, Result = result };
        }

        /// <summary>
        /// 从全量数据生成分页数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="page">分页数据</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, PageParameter page, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(source, nameof(source));
            Checker.CheckPageParameter(page, nameof(page));
            int count = await source.CountAsync(cancellationToken);
            if (page.PageSize <= 0)
            {
                return new PagedResult<T>() { Toltal = count, Result = Enumerable.Empty<T>().ToList(), };
            }
            IList<T> result = await source.Page(page).ToListAsync(cancellationToken);
            return new PagedResult<T>() { Toltal = count, Result = result, };
        }
    }
}
