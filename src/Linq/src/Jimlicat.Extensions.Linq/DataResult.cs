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
    public class DataResult<T> : IDataResult<T>
    {
        /// <summary>
        /// 数据结果
        /// </summary>
        public IList<T> Result { get; set; }
        /// <summary>
        /// 数据总量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 空 DataResult
        /// </summary>
        /// <returns></returns>
        public static DataResult<T> Empty()
        {
            return new DataResult<T>()
            {
                Result = Enumerable.Empty<T>().ToList(),
                Count = 0,
            };
        }
    }
}
