using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            var result = await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>() { Toltal = count, Result = result };
        }

        /// <summary>
        /// 从全量数据生成分页数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="page">分页数据</param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, PageParameter page)
        {
            int count = await source.CountAsync();
            IList<T> result;
            if (page == null)
            {
                result = await source.ToListAsync();
            }
            else
            {
                if (page.PageSize <= 0)
                {
                    return new PagedResult<T>() { Toltal = count, Result = Enumerable.Empty<T>().ToList(), };
                }
                result = await source.Page(page).ToListAsync();
            }
            return new PagedResult<T>() { Toltal = count, Result = result, };
        }
    }
}
