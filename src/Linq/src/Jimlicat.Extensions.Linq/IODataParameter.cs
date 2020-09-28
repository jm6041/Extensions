using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// OData 支持的参数封装
    /// </summary>
    public interface IODataParameter
    {
        /// <summary>
        /// $top
        /// </summary>
        int Top { get; set; }
        /// <summary>
        /// $skip
        /// </summary>
        int Skip { get; set; }
        /// <summary>
        /// $orderby
        /// </summary>
        string OrderBy { get; set; }
        /// <summary>
        /// $filter 在数据具体查询中实现
        /// </summary>
        string Filter { get; set; }
        /// <summary>
        /// $count 返回结果是否包含总数量
        /// </summary>
        bool Count { get; set; }
        /// <summary>
        /// 获得排序清单
        /// </summary>
        IReadOnlyDictionary<string, Direction> Orderings { get; }
    }
}
