using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace System.Linq
{
    static class PagingAsyncExtension
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
            return await Task.FromResult(new PagedResult<T>() { ToltalCount = count, Result = result });
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
            IList<T> result = null;
            if (page == null)
            {
                result = await source.ToListAsync();
            }
            else
            {
                if (page.PageSize <= 0)
                {
                    return new PagedResult<T>() { ToltalCount = count, Result = Enumerable.Empty<T>().ToList(), };
                }
                if (page.PageIndex < 0)
                {
                    page.PageIndex = 0;
                }
                result = await source.Page(page).ToListAsync();
            }
            return new PagedResult<T>() { ToltalCount = count, Result = result, };
        }
    }
}
