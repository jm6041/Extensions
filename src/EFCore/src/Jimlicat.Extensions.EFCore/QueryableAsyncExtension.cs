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
    public static class QueryableAsyncExtension
    {
        /// <summary>
        /// 从全量数据生成OData数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="para">分页数据</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        public static async Task<DataResult<T>> ToODataResultAsync<T>(this IQueryable<T> source, IODataParameter para, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(source, nameof(source));
            Checker.CheckODataParameter(para, nameof(para));
            int count = 0;
            if (para.Count)
            {
                count = await source.CountAsync(cancellationToken);
            }
            if (para.Top <= 0)
            {
                return new DataResult<T>() { Count = count, Result = Enumerable.Empty<T>().ToList(), };
            }
            IList<T> result = await source.ODataQuery(para).ToListAsync(cancellationToken);
            return new DataResult<T>() { Count = count, Result = result, };
        }
    }
}
