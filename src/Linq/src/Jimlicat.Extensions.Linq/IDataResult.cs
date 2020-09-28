using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 数据结果
    /// </summary>
    public interface IDataResult<T>
    {
        /// <summary>
        /// 数据结果
        /// </summary>
        IList<T> Result { get; }
        /// <summary>
        /// 数据总量
        /// </summary>
        int Count { get; }
    }
}
