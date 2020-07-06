using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            int count = await source.CountAsync(cancellationToken);
            var result = await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken);
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
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
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
