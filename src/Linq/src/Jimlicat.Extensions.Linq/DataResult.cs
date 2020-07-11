using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 数据结果
    /// </summary>
    /// <typeparam name="T">数据源类型</typeparam>
    [Serializable]
    public class DataResult<T>
    {
        /// <summary>
        /// 分页数据结果
        /// </summary>
        public IList<T> Result { get; set; }
        /// <summary>
        /// 数据总量
        /// </summary>
        public int Count { get; set; }
    }
}
