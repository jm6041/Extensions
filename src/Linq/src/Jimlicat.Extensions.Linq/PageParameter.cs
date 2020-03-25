using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 分页参数，PageIndex:DataMember(Order = 10000)
    /// PageSize:DataMember(Order = 10001)
    /// Orderings:DataMember(Order = 10002)
    /// </summary>
    [Serializable]
    [DataContract]
    public class PageParameter
    {
        /// <summary>
        /// 加载数据的页号  从0开始
        /// </summary>
        [DataMember(Order = 10000)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小，默认 20
        /// </summary>
        [DataMember(Order = 10001)]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序清单
        /// </summary>
        [DataMember(Order = 10002)]
        public ICollection<Ordering> Orderings { get; set; }
        /// <summary>
        /// 排序清单是否为空
        /// </summary>
        public bool OrderingsIsNullOrEmpty()
        {
            return Orderings == null || (!Orderings.Any());
        }
    }
}
